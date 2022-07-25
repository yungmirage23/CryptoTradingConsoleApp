using ConsoleCrypto.Models.Cryptocurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Models.Market
{
    internal class CryptoMarket
    {
        public string Name;
        public List<Tradeble> Tradebles=new();
        public CryptoMarket(string name, Market market)
        {
            Name = name;
            Market = market;
        }
        public Market Market { get; set; }
    }
    public enum Market
    {
        Testnet,
        Spot,
        Futures
    }
}
