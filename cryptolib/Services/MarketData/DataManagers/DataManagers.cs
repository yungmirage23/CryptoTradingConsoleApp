using Cryptodll.API;
using Cryptodll.WebSocket.WebSockets.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptolib.DataManagers
{
    public class DataManagers
    {
        public WebSocketManager ManagerWebSocket;
        public ApiManager  ManagerApi;
        public DataManagers(WebSocketManager managerWebSocket, ApiManager managerApi)
        {
            ManagerWebSocket = managerWebSocket;
            ManagerApi = managerApi;
        }
    }
}
