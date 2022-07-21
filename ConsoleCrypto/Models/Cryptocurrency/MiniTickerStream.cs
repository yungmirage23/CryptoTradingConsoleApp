using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Models.Cryptocurrency
{
    public class MiniTickerStream
    {
        [JsonProperty("e")]
        public string EventType;
        [JsonProperty("E")]
        public string EventTime;
        [JsonProperty("s")]
        public string Symbol;
        [JsonProperty("p")]
        public string PriceChange;
        [JsonProperty("P")]
        public string PriceChangePercent;
        [JsonProperty("w")]
        public string WeightedAveragePrice;
        [JsonProperty("c")]
        public string LastPrice;
        [JsonProperty("Q")]
        public string LastQuantity;
        [JsonProperty("b")]
        public string BestBidPrice;
        [JsonProperty("B")]
        public string BestBidQuantity;
        [JsonProperty("a")]
        public string BestAskPrice;
        [JsonProperty("A")]
        public string BestAskQuantity;
        [JsonProperty("o")]
        public string OpenPrice;
        [JsonProperty("h")]
        public string HighPrice;
        [JsonProperty("l")]
        public string LowPrice;
        [JsonProperty("v")]
        public string TotalBaseVolume;
        [JsonProperty("q")]
        public string TotalQuoteVolume;
    }
}
