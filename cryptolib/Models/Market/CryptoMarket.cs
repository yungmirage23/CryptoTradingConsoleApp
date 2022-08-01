using Cryptodll.Models.Cryptocurrency;

namespace Cryptodll.Models.CryptoMarket
{
    public abstract class CryptoMarket
    {
        public string Name;
        public abstract Task ConnectToMarketAsync(MarketEnum market);
        public abstract Task SubscribeToCoinDataAsync(Tradeble coin, MarketEnum market, int apiqLimit = 500);
    }
}
