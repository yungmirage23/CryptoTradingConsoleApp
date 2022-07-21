using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Server
{
    internal record ServerRequest(string Path,HttpMethod Method);
}
