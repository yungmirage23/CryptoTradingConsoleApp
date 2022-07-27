using System.Collections.Concurrent;

namespace Cryptodll.Models.Cryptocurrency
{
    public abstract class Tradeble
    {
        public string Name { get; }
        public string Method { get; }
        public Tradeble(string coinTicker, string method)
        {
            Name = coinTicker.ToLower();
            Method = method;
            onCoinReceived += CoinDataHandler;
        }

        public string[] timeframes = { "1m", "1h", "1d", "1w", "1M" };

        public ConcurrentDictionary<string, IEnumerable<KlineAPI>> TimeFrameKlines = new();
        public Stack<MiniTicker> tickerStreamsQueue =new Stack<MiniTicker>();
        
        public delegate void CoinReceivedHandler();
        public event CoinReceivedHandler onCoinReceived;

        public void PushToCoinStream(MiniTicker _miniStream)
        {
            tickerStreamsQueue.Push(_miniStream);
            //onCoinReceived();
        }
        private void CoinDataHandler()
        {
            ConsoleEx.Log(this.Name + '\t' + this.tickerStreamsQueue.Peek().ClosePrice + '\t' + this.tickerStreamsQueue.Count + '\t');
        }
    }
}