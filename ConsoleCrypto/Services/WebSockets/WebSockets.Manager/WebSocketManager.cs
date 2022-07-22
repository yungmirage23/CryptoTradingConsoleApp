using ConsoleCrypto.Models.Cryptocurrency;
using ConsoleCrypto.Services.WebSockets;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
//wss://fstream.binance.com/ws
//wss://stream.binancefuture.com/ws
namespace ConsoleCrypto.Services
{
    class WebSocketManager
    {
        List<Tradeble> tradebles;
        readonly Uri StonksUri = new Uri("wss://fstream.binance.com/ws");
        
        ClientWebSocket Client;

        CancellationTokenSource cancellationTokenSource=new CancellationTokenSource();
        CancellationToken tokenStream;

        public delegate void RecievedEventHandler(Stack<MiniTicker> stream);
        public event RecievedEventHandler onMessageReceived;


        public WebSocketManager(List<Tradeble> _tradebles)
        {
            tokenStream = cancellationTokenSource.Token;
            tradebles = _tradebles;            
        }

        public async Task ConnectToWebSocket()
        {
            Client = new ClientWebSocket();
            onMessageReceived += WriteStreamMessage;
            await Client.ConnectAsync(StonksUri, tokenStream);
            ConsoleEx.Log("WEB SOCKET MANAGER CONNECTED");
            var a =Task.Run(()=>ReceiveStreamMessageAsync(),tokenStream);
        }
        public async Task SubscribeCoinStreamAsync(Tradeble coin)
        {
            try
            {
                tradebles.Add(coin);
                var reqstring = coin.Name + coin.Method;
                var request = new RequestWebSocket();
                request.param.Add(reqstring);
                var stringText = JsonConvert.SerializeObject(request);
                request = null;
                await Client.SendAsync(Encoding.UTF8.GetBytes(stringText), WebSocketMessageType.Text, true, tokenStream);
                ConsoleEx.Log("WebSocketManager CoinAdded");
            }
            catch (Exception ex)
            {
                ConsoleEx.Log(ex);
            }
        }
        public async Task UnSubscribeCoinStream(Tradeble coin)
        {
            try
            {
                var request = new RequestWebSocket();
                request.method = "UNSUBSCRIBE";
                request.param.Add(coin.Name+coin.Method);
                var stringText = JsonConvert.SerializeObject(request);
                request=null;
                await Client.SendAsync(Encoding.UTF8.GetBytes(stringText),WebSocketMessageType.Text,true, tokenStream);
                if (tradebles.Count == 1)
                {
                    cancellationTokenSource.Cancel();
                    tradebles = null;
                    onMessageReceived -= WriteStreamMessage;
                    onMessageReceived = null ;
                    Client.Dispose();
                    ConsoleEx.Log("WEB SOCKET MANAGER DISPOSED");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                
            }
            catch (Exception ex)
            {
                ConsoleEx.Log(ex);
            }
        }
        private async Task ReceiveStreamMessageAsync()
        {
            ConsoleEx.Log("Started receiving tickers");
            while (!tokenStream.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                var buf = new byte[300];
                await Client.ReceiveAsync(buf,tokenStream);
                var resultString = Encoding.UTF8.GetString(buf);
                var resultObject = JsonConvert.DeserializeObject<MiniTicker>(resultString);
                if (resultObject.EventType != null)
                {
                    var stream = tradebles.FirstOrDefault(x => x.Name.ToLower() == resultObject.Symbol.ToLower());
                    stream.tickerStreamsQueue.Push(resultObject);
                    onMessageReceived(stream.tickerStreamsQueue);
                }
                else ConsoleEx.Log(resultString);
            }
            ConsoleEx.Log("Receiving tickers stoped");
        }

        private void WriteStreamMessage(Stack<MiniTicker> stream)
        {
            var tick = stream.Peek();
            ConsoleEx.Log(tick.Symbol + '\t' + tick.ClosePrice + '\t' + stream.Count + '\t');
        }
    }
}