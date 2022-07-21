using ConsoleCrypto.Models.Cryptocurrency;
using ConsoleCrypto.Services.WebSockets;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace ConsoleCrypto.Services
{
    public class WebSocketManager
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken tokenCoin = new CancellationToken();
        CancellationTokenSource cancellationTokenSource2 = new CancellationTokenSource();
        CancellationToken tokenStream=new CancellationToken();

        private List<ITradeble> tradebles;
        private readonly Uri StonksUri = new Uri("wss://fstream.binance.com/ws");
        //wss://fstream.binance.com/ws
        //wss://stream.binancefuture.com/ws
        private ClientWebSocket Client;

        public delegate void RecievedEventHandler(Stack<MiniTicker> stream);
        public event RecievedEventHandler onMessageReceived;


        public WebSocketManager(List<ITradeble> _tradebles)
        {
            tokenCoin = cancellationTokenSource.Token;
            tokenStream = cancellationTokenSource2.Token;
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
        public async Task SubscribeCoinStreamAsync(ITradeble coin)
        {
            try
            {
                tradebles.Add(coin);
                var reqstring = coin.Name + coin.Method;
                var request = new RequestWebSocket();
                request.param.Add(reqstring);
                var stringText = JsonConvert.SerializeObject(request);
                request = null;
                await Client.SendAsync(Encoding.UTF8.GetBytes(stringText), WebSocketMessageType.Text, true, tokenCoin);
                ConsoleEx.Log("WebSocketManager CoinAdded");
            }
            catch (Exception ex)
            {
                ConsoleEx.Log(ex);
            }
        }
        public async Task UnSubscribeCoinStream(ITradeble coin)
        {
            try
            {
                var request = new RequestWebSocket();
                request.method = "UNSUBSCRIBE";
                request.param.Add(coin.Name+coin.Method);
                var stringText = JsonConvert.SerializeObject(request);
                request=null;
                await Client.SendAsync(Encoding.UTF8.GetBytes(stringText),WebSocketMessageType.Text,true, tokenCoin);
                if (tradebles.Count == 1)
                {
                    cancellationTokenSource2.Cancel();
                    cancellationTokenSource.Cancel();
                    tradebles = null;
                    onMessageReceived -= WriteStreamMessage;
                    onMessageReceived = null ;
                    Client.Dispose();
                    ConsoleEx.Log("WEB SOCKET MANAGER DISPOSED");
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
                    var stream = tradebles.First(x => x.Name.ToLower() == resultObject.Symbol.ToLower());
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