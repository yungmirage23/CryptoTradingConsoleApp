using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.Market;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Cryptodll.API
{
    public static class RequestKlinesSnapshot
    {
        readonly static string ApiFutures = "https://fapi.binance.com";
        readonly static string ApiTestnet = "https://testnet.binancefuture.com";
        static NumberFormatInfo nfi = new NumberFormatInfo();
        
        public static async Task<IEnumerable<KlineAPI>> RequestData(string _symbol,string _interval,int _limit,Market _market=Market.Futures)
        {
            nfi.NumberDecimalSeparator = ".";
            using (HttpClient clinet= new HttpClient())
            {
                switch (_market)
                {
                    case Market.Testnet:
                        clinet.BaseAddress = new Uri(ApiTestnet);
                        break;
                    default:
                        clinet.BaseAddress = new Uri(ApiFutures);
                        break;
                }
                var query = $"fapi/v1/klines?symbol={_symbol.ToUpper()}&interval={_interval}&limit={_limit}";

                HttpResponseMessage response = (await clinet.GetAsync(query.ToString())).EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<string[]>>(stringResult);

                return result.Select(item => new KlineAPI
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
        }
    }
}
