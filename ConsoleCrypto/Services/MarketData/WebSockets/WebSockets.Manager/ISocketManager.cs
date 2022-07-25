using ConsoleCrypto.Models.Cryptocurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Services.API.WebSocket
{
    public interface ISocketManager
    {
        Task ConnectToWebSocket();
        Task DisconnectFromWebSocket();
    }
}
