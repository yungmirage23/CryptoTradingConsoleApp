using Cryptodll.API;
using Cryptodll.Models;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.WebSocket.WebSockets.Manager;
using cryptolib.Services.MarketData;
using cryptolib.Services.MarketData.DataManagers;

internal sealed class MarketDataEngine
{
    int _id = 0;
    Dictionary<MarketEnum, DataManagersWrapper> _managersDictionary=new();
    //connects to market (futures, spot , testnet)
    public async Task Connect(MarketEnum market, string socketUri, string apiUri)
    {
        if (!_managersDictionary.ContainsKey(market))
        {
            var socketManager = new WebSocketManager(_id, socketUri);
            var apiManager = new ApiManager(apiUri);

            await apiManager.Ping();
            await socketManager.ConnectToWebSocketAsync();

            _managersDictionary.Add(market, new DataManagersWrapper(apiManager, socketManager));

            _id++;
        }
        else
        {
            throw new InvalidOperationException("Engine has been already connected to this market");
        }
    }

    //subscribes to web socket stream to receive ticker to stack and requests and saves last klines for each timeframe given in coin
    public async Task StartCoinData(Tradeble coin, Dictionary<string, string> apiKlinesRequest, string webSocketRequest, MarketEnum market)
    {
        DataManagersWrapper dataManagers;
        _managersDictionary.TryGetValue(market, out dataManagers);

        if (dataManagers == null)
            throw new InvalidOperationException("Engine is not connected to this market or sequence contains no matching element");
        if (dataManagers.WebSocketManager.Tradebles.Contains(coin))
            throw new InvalidOperationException("Engine already connected to this coin");

        await dataManagers.WebSocketManager.SubscribeCoinStreamAsync(coin, webSocketRequest);

        var options = new ParallelOptions { MaxDegreeOfParallelism = 5 };
        await Parallel.ForEachAsync(apiKlinesRequest, options, async (timeframeUrl, token) =>
        {
            var snapshotKliens = await dataManagers.ApiManager.RequestKlinesAsync(timeframeUrl.Value);
            while (!coin.TimeFrameKlines.TryAdd(timeframeUrl.Key, snapshotKliens.ToList()))
            {
            }
        });
    }
    //finds coin in some market unsubscribes from coin stream and deletes data
    public async Task FinishCoinData(Tradeble coin,MarketEnum market)
    {
        DataManagersWrapper dataManagers;
        _managersDictionary.TryGetValue(market, out dataManagers);

        if (dataManagers == null)
            throw new InvalidOperationException("Engine is not connected to this market or sequence contains no matching element");

        if (dataManagers.WebSocketManager.Tradebles.Count() < 1 & !dataManagers.WebSocketManager.Tradebles.Contains(coin))
            throw new NullReferenceException($"Sequense not contains coin: {coin.Name}. Unsubscribing is impossible.");
       
        await dataManagers.WebSocketManager.UnSubscribeCoinStreamAsync(coin);
    }
}