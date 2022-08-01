using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptodll.WebSocket.WebSockets.Manager
{
    public interface ISocketManager
    {
        Task ConnectToWebSocket();
        Task DisconnectFromWebSocket();
    }
}
