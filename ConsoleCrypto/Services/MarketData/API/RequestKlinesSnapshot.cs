using ConsoleCrypto.Models.Cryptocurrency;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Services.API
{
    public static class RequestKlinesSnapshot
    {
        public readonly static string ApiFutures = "https://fapi.binance.com";
        public readonly static string ApiTestnet = "https://testnet.binancefuture.com";
        static NumberFormatInfo nfi = new NumberFormatInfo();
        private static IEnumerable<KlineAPI> CoinSnapshot;
        public static async Task<IEnumerable<KlineAPI>> RequestData(string symbol,string interval)
        {
            nfi.NumberDecimalSeparator = ".";
            string apiRoute = $"fapi/v1/klines?symbol={symbol.ToUpper()}&interval={interval}&limit=1";
            IEnumerable<KlineAPI> CoinSnap;
            using (HttpClient clinet= new HttpClient())
            {
                clinet.BaseAddress= new Uri(ApiFutures);
                HttpResponseMessage response = (await clinet.GetAsync(apiRoute)).EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<string[]>>(stringResult);
                CoinSnap = result.Select(item => new KlineAPI
                {
                    OpenTime = long.Parse(item[0], nfi),
                    Open = float.Parse(item[1], nfi),
                    High = float.Parse(item[2], nfi),
                    Low = float.Parse(item[3], nfi),
                    Close = float.Parse(item[4],nfi),
                    Volume = float.Parse(item[5], nfi),
                    CloseTime = long.Parse(item[6]),
                    QuoteAssetVolume =float.Parse(item[7], nfi),
                    NumberOfTrades = int.Parse(item[8]),
                    TakerBuyBaseAssetVolume = float.Parse(item[9], nfi),
                    TakerBuyQuoteAssetVolume = float.Parse(item[10], nfi),
                    Ignore = double.Parse(item[11]),
                });
            }
            return CoinSnap;
        }
    }
}
