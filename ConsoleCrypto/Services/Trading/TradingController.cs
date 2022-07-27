
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.Market;
using Cryptodll.Models.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Services.Trading
{
    internal class TradingController
    {
        private MarketDataEngine DataEngine;
        public TradingController(CryptoMarket market)
        {
            DataEngine=new MarketDataEngine(market);
        }
        

        public void AddMarket()
        {

        }
        public void DeleteMarket()
        {

        }


        public async Task StartReceivingCoin(Tradeble coin)
        {
            await DataEngine.StartCoinData(coin);
        }
        public async Task FinishReceivingCoin(Tradeble coin)
        {
            await DataEngine.FinishCoinData(coin);
        }
        public async Task StartTrading(Tradeble coin,IStrategy _strategy)
        {
            _strategy.StartStrategy(coin);
        }
    }
}
