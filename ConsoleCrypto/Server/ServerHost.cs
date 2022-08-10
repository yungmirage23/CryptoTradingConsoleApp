using ConsoleCrypto.Server.RequestParser;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

internal partial class ServerHost
{
    private readonly IHandler _handler;

    public ServerHost(IHandler handler)
    {
        _handler = handler;
    }

    public void StartV1()
    {
        ConsoleEx.Log("Server Started");
        TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 80);
        listener.Start();

        while (true)
        {
            try
            {
                using (var client = listener.AcceptTcpClient())
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream))
                {
                    var firstLine = reader.ReadLine();
                    for (string line = null; line != string.Empty; line = reader.ReadLine()) ;
                    var request = RequestParser.Parse(firstLine);
                    _handler.Handle(stream, request);
                }
            }
            catch (System.Exception ex)
            {
                ConsoleEx.Log(ex);
            }
        }
    }

    public void StartV2()
    {
        TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 80);
        listener.Start();

        while (true)
        {
            var client = listener.AcceptTcpClient();
            ProcessClient(client);
        }
    }

    public async Task StartAsync()
    {
        ConsoleEx.Log("Server Started Async");
        TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 80);
        listener.Start();

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            var _=ProcessClientAsync(client);
        }
    }

    private void ProcessClient(TcpClient client)
    {
        ThreadPool.QueueUserWorkItem(o =>
        {
            using (client)
            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream))
            {
                var firstLine = reader.ReadLine();
                for (string line = null; line != string.Empty; line = reader.ReadLine()) ;
                var request = RequestParser.Parse(firstLine);
                _handler.Handle(stream, request);
            }
        });
    }

    private async Task ProcessClientAsync(TcpClient client)
    {
        try
        {
            using (client)
            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream))
            {
                var firstLine = await reader.ReadLineAsync();
                for (string line = null; line != string.Empty; line = await reader.ReadLineAsync()) ;
                var request = RequestParser.Parse(firstLine);
                await _handler.HandleAsync(stream, request);
            }
        }
        catch (System.Exception ex)
        {
            ConsoleEx.Log(ex);
        }
    }
}