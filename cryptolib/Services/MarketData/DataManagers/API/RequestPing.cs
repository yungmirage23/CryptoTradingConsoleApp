using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptolib.DataManagers.API
{
    public class RequestPing
    {
        public int status; // 0: normal，1：system maintenance
        public string msg; // "normal", "system_maintenance"
        
    }
}
