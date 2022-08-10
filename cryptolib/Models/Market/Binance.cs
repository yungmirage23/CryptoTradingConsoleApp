using Cryptodll.Models;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.CryptoMarket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cryptolib.Models.Binance
{
    public class Binance : CryptoMarket
    {
        private MarketDataEngine DataEngine=new MarketDataEngine();

        //urls for binance ws and http connections
        private readonly string SocketTestUri = "wss://testnet.binance.vision/ws";
        private readonly string SocketSpotUri = "wss://stream.binance.com:9443/ws";
        private readonly string SocketFuturesUri = "wss://fstream.binance.com/ws";
        private readonly string ApiFuturesUri = "https://fapi.binance.com/fapi/v1";
        private readonly string ApiTestnetUri = "https://testnet.binancefuture.com/fapi/v1";
        private readonly string ApiSpotUri = "https://api.binance.com/api/v3";
        Uri url = new Uri("https://api.binance.com/sapi/v1");


        //checks possibility to connect to stonks server provides connection to market
        public override async Task ConnectToMarketAsync(MarketEnum market)
        {
            //switch different con strings for different markets type
            switch (market)
            {
                case MarketEnum.Spot:
                    await DataEngine.Connect(MarketEnum.Spot, SocketSpotUri, ApiSpotUri);
                    break;

                case MarketEnum.Futures:
                    await DataEngine.Connect(MarketEnum.Futures, SocketFuturesUri, ApiFuturesUri);
                    break;

                case MarketEnum.Testnet:
                    await DataEngine.Connect(MarketEnum.Testnet, SocketTestUri, ApiTestnetUri);
                    break;
            }
        }

        //builds urls for api and websocket managers
        public override async Task SubscribeToCoinDataAsync(Tradeble coin, MarketEnum market, int apiqLimit = 500)
        {
            Dictionary<string, string> apiKlinesRequests = new Dictionary<string, string>();
            string uri = $"/klines?symbol={coin.Name.ToUpper()}&limit={apiqLimit}";
            foreach (var tf in coin.timeframes)
            {
                apiKlinesRequests.Add(tf, uri + $"&interval={tf}");
            }
            var webSocketRequest = coin.Name + "@miniTicker";
            await DataEngine.StartCoinData(coin, apiKlinesRequests, webSocketRequest, market);
        }
        public override async Task UnsubscribeFromCoinDataAsync(Tradeble coin,MarketEnum market)
        {
            await DataEngine.FinishCoinData(coin,market);
        }

        public override async Task AccountData()
        {
            await DataEngine.GetAccountData(url, "account/apiTradingStatus", MarketEnum.Spot);
        }
    }
}