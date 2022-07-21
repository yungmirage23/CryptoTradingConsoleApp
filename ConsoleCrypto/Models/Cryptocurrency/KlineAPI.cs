using Newtonsoft.Json;

namespace ConsoleCrypto.Models.Cryptocurrency
{
    public class KlineAPI
    {
        public double OpenTime { get; set; }

        public string Open { get; set; }

        public string High { get; set; }

        public string Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }

        public string CloseTime { get; set; }

        public string QuoteAssetVolume { get; set; }

        public string NumberOfTrades { get; set; }

        public string TakerBuyBaseAssetVolume { get; set; }

        public string TakerBuyQuoteAssetVolume { get; set; }
        public string Ignore { get; set; }

    }
}