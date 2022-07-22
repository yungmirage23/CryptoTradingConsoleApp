using ConsoleCrypto.Models.Cryptocurrency;
using ConsoleCrypto.Models.Cryptocurrency.Coins;
using ConsoleCrypto.Services;
using ConsoleCrypto.Services.API;

internal sealed class TradingHost
{
    private WebSocketManager webSocketManager;
    
    private List<Tradeble> tradebles = new List<Tradeble>();
    FakeCoinsList fake =new FakeCoinsList();

    public async Task StartCoinTrading(Tradeble coin)
    {
        var snapshotKliens = await RequestKlinesSnapshot.RequestData(coin.Name, "1m");
        coin.coinKlines = snapshotKliens;
        snapshotKliens = null;
        if (webSocketManager == null)
        {
            ConsoleEx.Log("Host started");
            ConsoleEx.Log("WebSocketManager started");
            webSocketManager = new WebSocketManager(tradebles);
            await webSocketManager.ConnectToWebSocket();
        }
        ConsoleEx.Log("API DATA delivered");
        webSocketManager.SubscribeCoinStreamAsync(coin);
    }

    public async Task FinishCoinTrading(Tradeble coin)
    {
        await webSocketManager.UnSubscribeCoinStream(coin);
        if (tradebles.Count == 1)
        {
            webSocketManager = null;
            tradebles.Clear();
        }
        else tradebles.Remove(coin);
        coin = null;
    }

    public async Task ReadCommand()
    {
        int i = 0;
        while (true)
        {
            var a = Console.ReadKey();
            switch (a.KeyChar)
            {
                case 'q':
                    if (tradebles.Count == 0)
                    {
                        ConsoleEx.Log($"No more coins to trade");
                        break;
                    }
                    var coin = tradebles.Last();
                    await FinishCoinTrading(coin);
                    ConsoleEx.Log($"Coin {coin.Name} has been removed");
                    i--;
                    break;

                case 'e':
                    var coinfake = fake.fakeTradablesList[i];
                    if (i != fake.fakeTradablesList.Count - 1)
                    {
                        await StartCoinTrading(coinfake);
                        ConsoleEx.Log($"Coin {coinfake.Name} has been added");
                        i++;
                    }
                    else
                    {
                        ConsoleEx.Log($"No more coins to add");
                    }
                    break;
            }
        }
    }
}