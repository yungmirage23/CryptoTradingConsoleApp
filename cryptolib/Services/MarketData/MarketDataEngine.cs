using Cryptodll.API;
using Cryptodll.Models;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.WebSocket.WebSockets.Manager;
using cryptolib.DataManagers;

internal sealed class MarketDataEngine
{
    private int _id = 0;
    private WebSocketManager _webSocketManager;
    private Dictionary<MarketEnum, DataManagers> _webSockets = new();

    public async Task Connect(MarketEnum market, string socketUri, string apiUri)
    {
        if (!_webSockets.ContainsKey(market))
        {
            var socketManager = new WebSocketManager(_id, socketUri);
            var apiManager = new ApiManager(apiUri);
            await apiManager.Ping();
            await socketManager.ConnectToWebSocketAsync();
            _webSockets.Add(market, new DataManagers(socketManager, apiManager));
            _id++;
        }
        else
        {
            throw new InvalidOperationException("Engine has been already connected to this market");
        }
    }

    public async Task StartCoinData(Tradeble coin, Dictionary<string, string> apiKlinesRequest, string webSocketRequest, MarketEnum market)
    {
        DataManagers dataManagers;
        _webSockets.TryGetValue(market, out dataManagers);
        if (dataManagers == null)

            throw new InvalidOperationException("Engine is not connected to this market or sequence contains no matching element");

        await dataManagers.ManagerWebSocket.SubscribeCoinStreamAsync(coin, webSocketRequest);
        var options = new ParallelOptions { MaxDegreeOfParallelism = 5 };
        await Parallel.ForEachAsync(apiKlinesRequest, options, async (timeframeUrl, token) =>
        {
            var snapshotKliens = await dataManagers.ManagerApi.RequestKlinesAsync(timeframeUrl.Value);
            while (!coin.TimeFrameKlines.TryAdd(timeframeUrl.Key, snapshotKliens.ToList()))
            {
            }
        });
    }
    public async Task FinishCoinData(Tradeble _coin, string tickerName)
    {
        await _webSocketManager.UnSubscribeCoinStreamAsync(_coin, tickerName);
        _webSocketManager = null;
    }
}