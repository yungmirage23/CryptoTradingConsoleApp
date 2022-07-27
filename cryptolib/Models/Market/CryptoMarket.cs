using Cryptodll.Models.Cryptocurrency;

namespace Cryptodll.Models.Market
{
    public class CryptoMarket
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
