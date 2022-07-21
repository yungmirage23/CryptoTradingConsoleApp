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
        public static string ApiDomain = "https://testnet.binancefuture.com";
        static NumberFormatInfo nfi = new NumberFormatInfo();
        private static IEnumerable<KlineAPI> CoinSnapshot;
        public static async Task<IEnumerable<KlineAPI>> RequestData(string symbol,string interval)
        {
            nfi.NumberDecimalSeparator = ".";
            string apiRoute = $"fapi/v1/klines?symbol={symbol.ToUpper()}&interval={interval}";
            using (HttpClient clinet= new HttpClient())
            {
                clinet.BaseAddress= new Uri(ApiDomain);
                HttpResponseMessage response = (await clinet.GetAsync(apiRoute)).EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<string[]>>(stringResult);
                CoinSnapshot = result.Select(item => new KlineAPI
                {
                    OpenTime = double.Parse(item[0],nfi),
                    Open = item[1],
                    High = item[2],
                    Low = item[3],
                    Close = decimal.Parse(item[4],nfi),
                    Volume = decimal.Parse(item[5], nfi),
                    CloseTime = item[6],
                    QuoteAssetVolume = item[7],
                    NumberOfTrades = item[8],
                    TakerBuyBaseAssetVolume = item[9],
                    TakerBuyQuoteAssetVolume = item[10],
                    Ignore = item[11],
                });
            }
            return CoinSnapshot;
        }
    }
}
