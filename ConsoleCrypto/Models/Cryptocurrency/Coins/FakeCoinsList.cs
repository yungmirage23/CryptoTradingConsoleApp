using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Models.Cryptocurrency.Coins
{
    internal class FakeCoinsList
    {
        public List<Tradeble> fakeTradablesList;
        public FakeCoinsList()
        {
            fakeTradablesList = new List<Tradeble>()
            {
                new Coin("BTCUSDT", "@miniTicker"),
                new Coin("ETHUSDT", "@miniTicker"),
                new Coin("DOGEUSDT", "@miniTicker"),
                new Coin("LTCUSDT", "@miniTicker"),
                new Coin("XRPUSDT", "@miniTicker"),
                new Coin("FTMUSDT", "@miniTicker"),
            };
        }
    }
}
