using ConsoleCrypto.Models.Cryptocurrency;
using ConsoleCrypto.Models.Market;
using ConsoleCrypto.Services;
using ConsoleCrypto.Services.API;

internal sealed class MarketDataEngine
{
    private WebSocketManager webSocketManager;
    public CryptoMarket _market;
    
    public MarketDataEngine(CryptoMarket market)
    {
        _market = market;
    }
    public async Task StartCoinData(Tradeble coin)
    {
        if (webSocketManager == null)
        {
            _market.Tradebles = new();
            webSocketManager = new WebSocketManager(_market.Tradebles,_market.Market);
            await webSocketManager.ConnectToWebSocket();
            ConsoleEx.Log("Web Socket Manager started");
        }
        if (_market.Tradebles.Contains(coin))
        {
            return;
        }
        await webSocketManager.SubscribeCoinStreamAsync(coin);
        var options = new ParallelOptions { MaxDegreeOfParallelism = 10 };
        await Parallel.ForEachAsync(coin.timeframes,options,async(tf,token)=>
        {
            var snapshotKliens = await RequestKlinesSnapshot.RequestData(coin.Name, tf);
            coin.TimeFrameKlines.Add(tf, snapshotKliens);
            ConsoleEx.Log($"API {coin.Name} data for {tf} delivered");
        });
    }

    public async Task FinishCoinData(Tradeble _coin)
    {
        await webSocketManager.UnSubscribeCoinStream(_coin);
        webSocketManager = null;
    }
}