using System.Net.Http;

namespace ConsoleCrypto.Server
{
    public struct RequestApi
    {
        public string path;
        public string Api;
        public HttpMethod method;
        public RequestApi(string path, string api, HttpMethod method)
        {
            this.path = path;
            Api = api;
            this.method = method;
        }
    }
}