using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptodll.Models.Cryptocurrency
{
    public class CandleStream
    {
        [JsonProperty("e")]
        public string EventType;
        [JsonProperty("E")]
        public string EventTime;
        [JsonProperty("s")]
        public string Symbol;
        [JsonProperty("k")]
        public CandleStick Candle;

    }
}
