using Cryptodll.API;
using Cryptodll.WebSocket.WebSockets.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptolib.Services.MarketData.DataManagers
{
    public class DataManagersWrapper
    {
        public ApiManager ApiManager { get; set; }
        public WebSocketManager WebSocketManager { get; set; }
        public DataManagersWrapper(ApiManager manager, WebSocketManager websmanager)
        {
            ApiManager = manager;
            WebSocketManager = websmanager;
        }
    }
}
