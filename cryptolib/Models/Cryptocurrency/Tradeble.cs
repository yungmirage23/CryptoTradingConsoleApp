using System.Collections.Concurrent;

namespace Cryptodll.Models.Cryptocurrency
{
    public abstract class Tradeble
    {
        public string Name { get; }
        public Tradeble(string coinTicker)
        {
            Name = coinTicker.ToLower();
            onCoinReceived += CoinDataHandler;
        }

        public string[] timeframes = { "1m", "1h", /*"1d", "1w", "1M" */};

        public ConcurrentDictionary<string, List<KlineAPI>> TimeFrameKlines = new();
        public Stack<MiniTicker> tickerStreamsQueue =new Stack<MiniTicker>();
        
        public delegate void CoinReceivedHandler(MiniTicker MiniTicker);
        public event CoinReceivedHandler? onCoinReceived;

        public void PushToCoinStream(MiniTicker _miniTick)
        {
            tickerStreamsQueue.Push(_miniTick);
            if(onCoinReceived != null)
            onCoinReceived(_miniTick);
        }
        private void CoinDataHandler(MiniTicker miniTicker)
        {
            Console.WriteLine(this.Name + '\t' + this.tickerStreamsQueue.Peek().ClosePrice + '\t' + this.tickerStreamsQueue.Count + '\t');
        }
    }
}