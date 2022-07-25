using ConsoleCrypto.Models.Cryptocurrency;

namespace ConsoleCrypto.Models.Strategy
{
    internal class LevelsStrategy : IStrategy
    {
        private struct LowHighData
        {
            public string LowPrice;
            public string HighPrice;
        }

        private float risk;
        private Dictionary<string, LowHighData> TimeFrameMaxMin = new();

        public LevelsStrategy()
        {
        }

        public void StartStrategy(Tradeble coin)
        {
            FindCoinHighLow(coin);
        }

        private void Buy(Tradeble coin)
        {
            ConsoleEx.Log($"\tBought {coin.Name} on price {coin.tickerStreamsQueue.Peek().ClosePrice}");
        }

        private void Sell(Tradeble coin)
        {
            ConsoleEx.Log($"\tSold {coin.Name} on price {coin.tickerStreamsQueue.Peek().ClosePrice}");
        }

        private void FindCoinHighLow(Tradeble coin)
        {
            Parallel.ForEach(coin.TimeFrameKlines, klines =>
            {
                var maxval = klines.Value.Aggregate((i1, i2) => i1.High > i2.High ? i1 : i2);
                var minval = klines.Value.Aggregate((i1, i2) => i1.High < i2.High ? i1 : i2);
                ConsoleEx.Log($"{coin.Name} tf:{klines.Key} max: {maxval.High} min:{minval.Low}");
            });
        }
    }
}