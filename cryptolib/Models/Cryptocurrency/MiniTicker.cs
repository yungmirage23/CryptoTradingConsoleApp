using Newtonsoft.Json;

namespace Cryptodll.Models.Cryptocurrency
{
    public class MiniTicker
    {
        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public string EventTime { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("c")]
        public decimal ClosePrice { get; set; }

        [JsonProperty("o")]
        public string OpenPrice { get; set; }

        [JsonProperty("h")]
        public string HighPrice { get; set; }

        [JsonProperty("l")]
        public string LowPrice { get; set; }
    }
}