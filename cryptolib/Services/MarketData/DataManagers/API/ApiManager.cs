using Cryptodll.Models.Cryptocurrency;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Cryptodll.API
{
    public class ApiManager
    {
        string _baseAddress;
        # region development secret 
        string _second;        
        string _first;
        #endregion
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
        public async Task<T> GetAsync<T>(string endpoint)
        {
            using (HttpClient clinet = new HttpClient())
            {
                var quer = _baseAddress + endpoint;
                var response = await clinet.GetAsync(_baseAddress+endpoint);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(response.StatusCode.ToString());

                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
        }
        public async Task<T> GetSignedAsync<T>(Uri uri,string endpoint,string args=null)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            using (var reader = new StreamReader(path+"/fiel.txt", false))
            {
                reader.ReadLine();
                reader.ReadLine();
                _first = reader.ReadLine();
                reader.ReadLine();
                _second = reader.ReadLine();
            }
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                var headers = client.DefaultRequestHeaders.ToString();
                var timestamp = GetTimestamp();
                args += "&timestamp=" + timestamp;
                var signature = args.CreateSignature(_second);
                var response = await client.GetAsync($"{endpoint}?{args}&signature={signature}");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(response.StatusCode.ToString());
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
        }
        private string GetTimestamp()
        {
            var timelong = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            return timelong.ToString();
        }
    }
    public static class Helpers
    {
        private static readonly Encoding SignatureEncoding = Encoding.UTF8;
        public static string CreateSignature(this string message,string secret)
        {
            byte[] secretbytes= SignatureEncoding.GetBytes(secret);
            byte[] messagebytes = SignatureEncoding.GetBytes(message);
            HMACSHA256 hmacsha256 =new HMACSHA256(secretbytes);
            byte[] bytes = hmacsha256.ComputeHash(messagebytes);
            return BitConverter.ToString(bytes).Replace("-","").ToLower();
        }
    }
}