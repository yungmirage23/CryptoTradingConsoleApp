using Cryptodll.API;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.Market;
using Cryptodll.WebSocket.WebSockets.Manager;

internal sealed class MarketDataEngine
{
    private WebSocketManager webSocketManager;
    public CryptoMarket _market;
    
    public MarketDataEngine(CryptoMarket market)
    {
        _market = market;
    }
    public async Task StartCoinData(Tradeble _coin)
    {
        if (webSocketManager == null)
        {
            _market.Tradebles = new();
            webSocketManager = new WebSocketManager(_market.Tradebles,_market.Market);
            await webSocketManager.ConnectToWebSocket();
            ConsoleEx.Log("Web Socket Manager started");
        }
        if (_market.Tradebles.Contains(_coin))
        {
            return;
        }
        await webSocketManager.SubscribeCoinStreamAsync(_coin);
        var options = new ParallelOptions { MaxDegreeOfParallelism = 5 };
        await Parallel.ForEachAsync(_coin.timeframes, options, async (tf, token) =>
        {
            var snapshotKliens = await RequestKlinesSnapshot.RequestData(_coin.Name, tf, 500);            
            while (!_coin.TimeFrameKlines.TryAdd(tf, snapshotKliens.ToList()))
            {

            }
            ConsoleEx.Log($"API {_coin.Name} data for {tf} delivered");
        });
    }

    public async Task FinishCoinData(Tradeble _coin)
    {
        await webSocketManager.UnSubscribeCoinStream(_coin);
        webSocketManager = null;
    }
}