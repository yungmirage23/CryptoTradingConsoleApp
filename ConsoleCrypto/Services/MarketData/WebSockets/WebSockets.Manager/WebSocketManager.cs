using ConsoleCrypto.Models.Cryptocurrency;
using ConsoleCrypto.Models.Market;
using ConsoleCrypto.Services.WebSockets;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using static MarketDataEngine;

//wss://fstream.binance.com/ws
//wss://stream.binancefuture.com/ws
namespace ConsoleCrypto.Services
{
    internal class WebSocketManager
    {
        private List<Tradeble> _tradebles;

        private readonly Uri TestUri = new Uri("wss://testnet.binance.vision/ws");
        private readonly Uri SpotUri = new Uri("wss://stream.binance.com:9443/ws");
        private readonly Uri FuturesUri = new Uri("wss://fstream.binance.com/ws");

        private Market Market;
        private ClientWebSocket client;

        private Semaphore sem = new Semaphore(1, 1);

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private CancellationToken tokenStream;

        public WebSocketState State=WebSocketState.Closed;

        public WebSocketManager(List<Tradeble> tradebles, Market market)
        {
            tokenStream = cancellationTokenSource.Token;
            _tradebles = tradebles;
            Market = market;
        }

        public async Task ConnectToWebSocket()
        {
            client = new ClientWebSocket();
            State = WebSocketState.Connecting;
            switch (Market)
            {
                case Market.Testnet:
                    await client.ConnectAsync(TestUri, tokenStream);
                    break;

                case Market.Spot:
                    await client.ConnectAsync(SpotUri, tokenStream);
                    break;

                case Market.Futures:
                    await client.ConnectAsync(FuturesUri, tokenStream);
                    break;
            }
            State= WebSocketState.Open;
            var a = Task.Run(() => ReceiveStreamMessageAsync(), tokenStream);
        }

        public async Task SubscribeCoinStreamAsync(Tradeble coin)
        {
            try
            {
                _tradebles.Add(coin);
                var reqstring = coin.Name + coin.Method;
                var request = new WebSocketRequest();
                request.param.Add(reqstring);
                var stringText = JsonConvert.SerializeObject(request);
                request = null;
                await client.SendAsync(Encoding.UTF8.GetBytes(stringText), WebSocketMessageType.Text, true, tokenStream);
            }
            catch (Exception ex)
            {
                ConsoleEx.Log(ex);
            }
        }

        public async Task UnSubscribeCoinStream(Tradeble coin)
        {
            sem.WaitOne();
            try
            {
                var request = new WebSocketRequest();
                request.method = "UNSUBSCRIBE";
                request.param.Add(coin.Name + coin.Method);
                var stringText = JsonConvert.SerializeObject(request);
                request = null;
                await client.SendAsync(Encoding.UTF8.GetBytes(stringText), WebSocketMessageType.Text, true, tokenStream);
                _tradebles.Remove(coin);
                if (_tradebles.Count == 0)
                {
                    cancellationTokenSource.Cancel();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                ConsoleEx.Log(ex);
            }
            sem.Release();
        }

        private async Task ReceiveStreamMessageAsync()
        {
            while (!tokenStream.IsCancellationRequested)
            {
                sem.WaitOne();
                try
                {
                    WebSocketReceiveResult result;
                    var buf = new byte[300];
                    await client.ReceiveAsync(buf, tokenStream);
                    var resultString = Encoding.UTF8.GetString(buf);
                    var resultObject = JsonConvert.DeserializeObject<MiniTicker>(resultString);
                    if (resultObject.EventType != null)
                    {
                        var coin = _tradebles.FirstOrDefault(x => x.Name.ToLower() == resultObject.Symbol.ToLower());
                        if(coin != null)
                        coin.PushToCoinStream(resultObject);
                    }
                    else ConsoleEx.Log(resultString);
                }
                catch (TaskCanceledException ex)
                {
                    ConsoleEx.Log("Task was canseled");
                }
                sem.Release();
            }
            client.Dispose();
            State = WebSocketState.Closed;
        }
    }
}