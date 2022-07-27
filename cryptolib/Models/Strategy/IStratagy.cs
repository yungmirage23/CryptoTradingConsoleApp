using Cryptodll.Models.Cryptocurrency;

namespace Cryptodll.Models.Strategy
{
    public interface IStrategy
    {
        public void StartStrategy(Tradeble coin);
    }
}
