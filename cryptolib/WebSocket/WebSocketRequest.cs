using Newtonsoft.Json;
namespace Cryptodll.WebSockets
{
    public record WebSocketRequest
    {
        public string method = "SUBSCRIBE";
        [JsonProperty("params")]
        public List<string> param = new List<string>();
        public int id = 1;
    }
}