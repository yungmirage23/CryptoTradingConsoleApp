using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.Strategy;

namespace cryptolib.Models.Strategy.LevelsStrategy
{
    public class LevelsStrategy : IStrategy
    {
        public Stack<Level> HighLevels;
        public Stack<Level> LowLevels;
        public LevelsStrategy()
        {
        }

        public void StartStrategy(Tradeble _coin)
        {
            CreateLevels(_coin, 5);
            //_coin.onCoinReceived +=;
        }

        private void Buy(Tradeble _coin)
        {
        }

        private void Sell(Tradeble _coin)
        {
        }

        private void CreateLevels(Tradeble _coin, int _coefficient)
        {
            List<Level> Levels = new();
            foreach (var pair in _coin.TimeFrameKlines)
            {
                int weight = 1;
                switch (pair.Key)
                {
                    case "1m":
                        weight = 1;
                        break;

                    case "1h":
                        weight = 2;
                        break;

                    case "1d":
                        weight = 3;
                        break;

                    case "1w":
                        weight = 4;
                        break;

                    case "1M":
                        weight = 5;
                        break;
                }
                var list = pair.Value.ToArray();
                int current = list.Length - 1;

                var klineStart = list[current];
                float max = klineStart.High;
                float min = klineStart.Low;

                int mincounter = 0;
                int maxcounter = 0;

                while (current != 0)
                {
                    var curr = list[--current];
                    var currenthigh = curr.High;
                    var currentlow = curr.Low;
                    Parallel.Invoke(
                        () =>
                    {
                        if (currenthigh > max)
                        {
                            max = currenthigh;
                            maxcounter = 0;
                        }
                        else maxcounter++;
                        if (maxcounter == _coefficient)
                        {
                            Levels.Add(new Level(max, LevelPurpose.Resistance, weight));
                        }
                    },
                        () =>
                    {
                        if (currentlow < min)
                        {
                            min = currentlow;
                            mincounter = 0;
                        }
                        else mincounter++;
                        if (mincounter == _coefficient)
                        {
                            Levels.Add(new Level(min, LevelPurpose.Support, weight));
                        }
                    });
                }
            }
            SortLevels(Levels);
        }
        private void SortLevels(List<Level> _levels)
        {
            var resistanse = _levels.Where(x => x.Purpose == LevelPurpose.Resistance).DistinctBy(x => x.Price).OrderByDescending(x => x.Price);
            var support = _levels.Where(x => x.Purpose == LevelPurpose.Support).DistinctBy(x => x.Price).OrderBy(x => x.Price);
            HighLevels = new Stack<Level>(resistanse);
            LowLevels = new Stack<Level>(support);
        }

    }

}