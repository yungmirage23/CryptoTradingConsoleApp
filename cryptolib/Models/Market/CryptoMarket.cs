using Cryptodll.Models.Cryptocurrency;

namespace Cryptodll.Models.CryptoMarket
{
    public abstract class CryptoMarket
    {
        public abstract Task ConnectToMarketAsync(MarketEnum market);
        public abstract Task SubscribeToCoinDataAsync(Tradeble coin, MarketEnum market, int apiqLimit = 500);
        public abstract Task UnsubscribeFromCoinDataAsync(Tradeble coin, MarketEnum market);
    }
}
