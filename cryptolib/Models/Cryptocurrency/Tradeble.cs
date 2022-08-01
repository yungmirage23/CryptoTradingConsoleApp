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

        // holds data about last {limit}[500-2000] klines for every timeframe
        public ConcurrentDictionary<string, List<KlineAPI>> TimeFrameKlines = new();

        //stack whitch receives data about last ticker price
        public Stack<MiniTicker> tickerStreamsQueue =new Stack<MiniTicker>();
        
        //fires then websocket manager receives data about this tradeble
        public delegate void CoinReceivedHandler(MiniTicker MiniTicker);
        public event CoinReceivedHandler? onCoinReceived;

        //pushes data to stack and provokes event
        public void PushToCoinStream(MiniTicker _miniTick)
        {
            tickerStreamsQueue.Push(_miniTick);
            if(onCoinReceived != null)
            onCoinReceived(_miniTick);
        }

        //for test
        private void CoinDataHandler(MiniTicker miniTicker)
        {
            Console.WriteLine(this.Name + '\t' + this.tickerStreamsQueue.Peek().ClosePrice + '\t' + this.tickerStreamsQueue.Count + '\t');
        }
    }
}