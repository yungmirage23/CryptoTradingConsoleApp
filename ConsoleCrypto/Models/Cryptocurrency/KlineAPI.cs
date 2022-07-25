﻿using Newtonsoft.Json;

namespace ConsoleCrypto.Models.Cryptocurrency
{
    public class KlineAPI
    {
        public long OpenTime { get; set; }

        public float Open { get; set; }

        public float High { get; set; }

        public float Low { get; set; }

        public float Close { get; set; }

        public float Volume { get; set; }

        public long CloseTime { get; set; }

        public float QuoteAssetVolume { get; set; }

        public float NumberOfTrades { get; set; }

        public float TakerBuyBaseAssetVolume { get; set; }

        public float TakerBuyQuoteAssetVolume { get; set; }
        public double Ignore { get; set; }

    }
}