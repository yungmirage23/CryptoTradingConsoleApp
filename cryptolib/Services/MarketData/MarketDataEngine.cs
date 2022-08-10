using Cryptodll.API;
using Cryptodll.Models;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.WebSocket.WebSockets.Manager;
using cryptolib.Services.MarketData;
using cryptolib.Services.MarketData.DataManagers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

internal sealed class MarketDataEngine
{
    int _id = 0;
    Dictionary<MarketEnum, DataManagersWrapper> _managersDictionary=new Dictionary<MarketEnum, DataManagersWrapper>();
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
       
        NumberFormatInfo _nfi = new NumberFormatInfo();
        _nfi.NumberDecimalSeparator = ".";
        Parallel.ForEach(apiKlinesRequest,async (timeframeUrl, token) =>
        {
            var snapshotKliensStrings =await dataManagers.ApiManager.GetAsync<IEnumerable<string[]>>(timeframeUrl.Value);
            var snapshotKliens = snapshotKliensStrings.Select(item => new KlineAPI
            {
                OpenTime = long.Parse(item[0], _nfi),
                Open = float.Parse(item[1], _nfi),
                High = float.Parse(item[2], _nfi),
                Low = float.Parse(item[3], _nfi),
                Close = float.Parse(item[4], _nfi),
                Volume = float.Parse(item[5], _nfi),
                CloseTime = long.Parse(item[6]),
                QuoteAssetVolume = float.Parse(item[7], _nfi),
                NumberOfTrades = int.Parse(item[8]),
                TakerBuyBaseAssetVolume = float.Parse(item[9], _nfi),
                TakerBuyQuoteAssetVolume = float.Parse(item[10], _nfi),
                Ignore = double.Parse(item[11]),
            });
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
    public async Task GetAccountData(Uri uri,string endpoint,MarketEnum market)
    {
        DataManagersWrapper dataManagers;
        _managersDictionary.TryGetValue(market, out dataManagers);
        if (dataManagers == null)
            throw new InvalidOperationException("Engine is not connected to this market or sequence contains no matching element");
        await dataManagers.ApiManager.GetSignedAsync<IEnumerable<string[]>>(uri,endpoint);
    }
}