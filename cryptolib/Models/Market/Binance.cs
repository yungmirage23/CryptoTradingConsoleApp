using Cryptodll.Models;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.CryptoMarket;

namespace cryptolib.Models.Binance
{
    public class Binance : CryptoMarket
    {
        private MarketDataEngine marketDataEngine = new();

        private readonly string SocketTestUri = "wss://testnet.binance.vision/ws";
        private readonly string SocketSpotUri = "wss://stream.binance.com:9443/ws";
        private readonly string SocketFuturesUri = "wss://fstream.binance.com/ws";
        private readonly string ApiFuturesUri = "https://fapi.binance.com/fapi/v1";
        private readonly string ApiTestnetUri = "https://testnet.binancefuture.com/fapi/v1";
        private readonly string ApiSpotUri = "https://api.binance.com/api/v3";

        public override async Task ConnectToMarketAsync(MarketEnum market)
        {
            switch (market)
            {
                case MarketEnum.Spot:
                    await marketDataEngine.Connect(MarketEnum.Spot, SocketSpotUri, ApiSpotUri);
                    break;

                case MarketEnum.Futures:
                    await marketDataEngine.Connect(MarketEnum.Futures, SocketFuturesUri, ApiFuturesUri);
                    break;

                case MarketEnum.Testnet:
                    await marketDataEngine.Connect(MarketEnum.Testnet, SocketTestUri, ApiTestnetUri);
                    break;
            }
        }

        public override async Task SubscribeToCoinDataAsync(Tradeble coin, MarketEnum market, int apiqLimit = 500)
        {
            Dictionary<string, string> apiKlinesRequests = new();
            string uri = $"/klines?symbol={coin.Name.ToUpper()}&limit={apiqLimit}";
            foreach (var tf in coin.timeframes)
            {
                apiKlinesRequests.Add(tf, uri + $"&interval={tf}");
            }
            var webSocketRequest = coin.Name + "@miniTicker";
            await marketDataEngine.StartCoinData(coin, apiKlinesRequests, webSocketRequest, market);
        }
    }
}