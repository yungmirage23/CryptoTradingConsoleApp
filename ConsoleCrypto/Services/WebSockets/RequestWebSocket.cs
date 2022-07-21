using Newtonsoft.Json;
namespace ConsoleCrypto.Services.WebSockets
{
    public record RequestWebSocket
    {
        public string method = "SUBSCRIBE";
        [JsonProperty("params")]
        public List<string> param = new List<string>();
        public int id = 1;
    }
}
