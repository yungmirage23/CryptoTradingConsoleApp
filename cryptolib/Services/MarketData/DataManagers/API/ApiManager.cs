using Cryptodll.Models.Cryptocurrency;
using Newtonsoft.Json;
using System.Globalization;

namespace Cryptodll.API
{
    public class ApiManager
    {
        static NumberFormatInfo _nfi = new NumberFormatInfo();

        string _baseAddress;
        public ApiManager(string baseurl)
        {
            _baseAddress = baseurl;
        }
        public async Task Ping()
        {
            using (HttpClient client = new HttpClient())
            {
                var query = "/ping";
                HttpResponseMessage response = (await client.GetAsync(_baseAddress+query));
                if(!response.IsSuccessStatusCode)
                    throw new Exception("Server is not responsing");
            }
        }
        //makes api request to take [500-2000] last klines for each timeframe , desereilize it and save to dictionary 
        public async Task<IEnumerable<KlineAPI>> RequestKlinesAsync(string url)
        {
            _nfi.NumberDecimalSeparator = ".";
            using (HttpClient clinet = new HttpClient())
            {
                var quer = _baseAddress + url;
                HttpResponseMessage response = (await clinet.GetAsync(_baseAddress+url)).EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<string[]>>(stringResult);

                return result.Select(item => new KlineAPI
                {
                    OpenTime = long.Parse(item[0], _nfi),
                    Open = float.Parse(item[1], _nfi),
                    High = float.Parse(item[2], _nfi),
                    Low = float.Parse(item[3], _nfi),
                    Close = float.Parse(item[4], _nfi),
                    Volume = float.Parse(item[5], _nfi),
                    CloseTime = long.Parse(item[6]),
                    QuoteAssetVolume = float.Parse(item[7], _nfi),
                    NumberOfTrades = int.Parse(item[8]),
                    TakerBuyBaseAssetVolume = float.Parse(item[9], _nfi),
                    TakerBuyQuoteAssetVolume = float.Parse(item[10], _nfi),
                    Ignore = double.Parse(item[11]),
                });
            }
        }
    }
}