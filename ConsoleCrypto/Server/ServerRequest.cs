using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Server
{
    public struct ServerRequest
    {
        public string Path;
        public HttpMethod Method;
        public ServerRequest(string path,HttpMethod method)
        {
            Path = path;
            Method = method;
        }
    }

    
}
