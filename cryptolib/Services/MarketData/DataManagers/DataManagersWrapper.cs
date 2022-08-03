using Cryptodll.API;
using Cryptodll.WebSocket.WebSockets.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptolib.Services.MarketData.DataManagers
{
    public record class DataManagersWrapper
    {
        public ApiManager ApiManager;
        public WebSocketManager WebSocketManager;
        public DataManagersWrapper(ApiManager manager, WebSocketManager websmanager)
        {
            ApiManager = manager;
            WebSocketManager = websmanager;
        }
    }
}
