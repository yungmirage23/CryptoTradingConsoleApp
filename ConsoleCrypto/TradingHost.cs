using ConsoleCrypto.Models.Cryptocurrency;
using ConsoleCrypto.Services;
using ConsoleCrypto.Services.API;

internal class TradingHost
{
    private WebSocketManager webSocketManager;
    private Coin BTCUSDT = new Coin("BTCUSDT", "@miniTicker");
    private Coin ETHUSDT = new Coin("ETHUSDT", "@miniTicker");
    private Coin DOGEUSDT = new Coin("DOGEUSDT", "@miniTicker");
    private Coin LTCUSDT = new Coin("LTCUSDT", "@miniTicker");
    private Coin XRPUSDT = new Coin("XRPUSDT", "@miniTicker");
    private Coin FTMUSDT = new Coin("FTMUSDT", "@miniTicker");
    private List<ITradeble> tradebles = new List<ITradeble>();
    private List<ITradeble> fakeTradablesList = new List<ITradeble>();

    public TradingHost()
    {
        fakeTradablesList.Add(BTCUSDT);
        fakeTradablesList.Add(ETHUSDT);
        fakeTradablesList.Add(DOGEUSDT);
        fakeTradablesList.Add(FTMUSDT);
        fakeTradablesList.Add(XRPUSDT);
        fakeTradablesList.Add(LTCUSDT);
    }

    public async Task StartCoinTrading(ITradeble coin)
    {
        var snapshotKliens = await RequestKlinesSnapshot.RequestData(coin.Name, "1m");

        if (webSocketManager == null)
        {
            ConsoleEx.Log("Host started");
            ConsoleEx.Log("WebSocketManager started");
            webSocketManager = new WebSocketManager(tradebles);
            await webSocketManager.ConnectToWebSocket();
        }
        coin.coinKlines = snapshotKliens;
        ConsoleEx.Log("API DATA delivered");
        webSocketManager.SubscribeCoinStreamAsync(coin);
    }

    public async Task FinishCoinTrading(ITradeble coin)
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
                    var coinfake = fakeTradablesList[i];
                    if (i != fakeTradablesList.Count - 1)
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

    public void Exit()
    {
    }
}

//class TradingController
//{
//    List<ITradebl> Coens;
//    public TradingController(List<ITradebl> _coens)
//    {
//        Coens = _coens;
//    }
//    public void SubscribeToStreams()
//    {
//        foreach(var coen in Coens)
//        {
//            ConsoleEx.Log($"{coen.Name} stream subscribed");
//        }
//    }
//}
//interface ITradebl
//{
//    public string Name { get; set; }
//    public void MakeSnapshot();
//    public void StartStrategy();
//}
//class Coen:ITradebl
//{
//    public IStratagy Stratagy;
//    public string Name { get; set; }
//    public string Method { get; set; }
//    List<KlineAPI> coens;
//    public Coen(string _name,string _method,IStratagy _stratagy)
//    {
//        Name = _name;
//        Method = _method;
//        Stratagy = _stratagy;
//    }
//    public void MakeSnapshot()
//    {
//        ConsoleEx.Log($"Coin:{Name} Strategy:{Stratagy.Name} snapshot made");
//    }
//    public void StartStrategy()
//    {
//        Stratagy.LoadStrategy();
//    }

//}
//interface IStratagy
//{
//    public string Name { get; }
//    public void LoadStrategy();
//}
//class LevelsStratagy : IStratagy
//{
//    public string Name{ get { return "LevelsStrategy"; } }
//    public void LoadStrategy()
//    {
//        ConsoleEx.Log("LevelsStrategyStarted");
//    }
//}
//class FastTrade : IStratagy
//{
//    public string Name { get { return "FastTrade"; } }
//    public void LoadStrategy()
//    {
//        ConsoleEx.Log("FastStrategyStarted");
//    }
//}