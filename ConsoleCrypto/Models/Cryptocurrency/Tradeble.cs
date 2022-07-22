namespace ConsoleCrypto.Models.Cryptocurrency
{
    public abstract class Tradeble
    {
        public IEnumerable<KlineAPI> coinKlines { get; set; }
        public Stack<MiniTicker> tickerStreamsQueue =new Stack<MiniTicker>();
        public string Name { get; }
        public string Method { get; }
        public Tradeble(string coinTicker, string method) 
        {
            Name = coinTicker.ToLower();
            Method = method;
        }
    }
}