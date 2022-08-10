using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Server.RequestParser
{
    internal static class RequestParser
    {
        public static ServerRequest Parse(string header)
        {
            var split=header.Split(' ');
            return new ServerRequest(split[1], GetMethod(split[0]));
        }
        private static HttpMethod GetMethod(string method)
        {
            if(method == "GET")
            {
                return HttpMethod.Get;
            }
            return HttpMethod.Post;
        }
    }
}
