namespace ConsoleCrypto.Models.Cryptocurrency
{
    public interface ITradeble
    {
        public IEnumerable<KlineAPI> coinKlines { get; set; }
        public Stack<MiniTicker> tickerStreamsQueue { get; }
        public string Name { get; }
        public string Method { get; }
        public void MakeCoinSnapshot();
        public void ConnectToCoinStream();
    }
}