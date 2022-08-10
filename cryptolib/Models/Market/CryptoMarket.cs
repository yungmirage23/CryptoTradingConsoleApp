using Cryptodll.Models.Cryptocurrency;
using cryptolib.Services.MarketData.DataManagers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptodll.Models.CryptoMarket
{
    public abstract class CryptoMarket
    {
        public Dictionary<MarketEnum, DataManagersWrapper> Data = new Dictionary<MarketEnum, DataManagersWrapper>();
        public abstract Task ConnectToMarketAsync(MarketEnum market);
        public abstract Task SubscribeToCoinDataAsync(Tradeble coin, MarketEnum market, int apiqLimit = 500);
        public abstract Task UnsubscribeFromCoinDataAsync(Tradeble coin, MarketEnum market);
        public abstract Task AccountData();
    }
}
