using Newtonsoft.Json;
using System.Collections.Generic;

namespace Cryptodll.WebSockets
{
    public class WebSocketRequest
    {
        public string method="SUBSCRIBE";
        [JsonProperty("params")]
        public List<string> param=new List<string>();
        public int id=1;

    }
}