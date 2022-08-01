﻿using Cryptodll.Models.Cryptocurrency;
using Cryptodll.WebSockets;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

//wss://fstream.binance.com/ws
//wss://stream.binancefuture.com/ws
namespace Cryptodll.WebSocket.WebSockets.Manager
{
    public class WebSocketManager
    {
        public List<Tradeble> _tradebles=new();
        //just increment for not repeating id
        int _id;
        ClientWebSocket _client;
        //semaphore which not allows to receive data to deleted list after unsubscribe from stream
        Semaphore _sem = new Semaphore(1, 1);

        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken _tokenStream;

        public WebSocketState State = WebSocketState.Closed;
        string _baseUrl;
        public WebSocketManager(int id,string baseUrl)
        {
            _tokenStream = _cancellationTokenSource.Token;
            _id = id;
            _baseUrl = baseUrl;
        }

        //tries to connect to web socket
        public async Task ConnectToWebSocketAsync()
        {
            _client = new ClientWebSocket();
            try
            {
               await _client.ConnectAsync(new Uri(_baseUrl), _tokenStream);
            }
            catch(Exception ex)
            {
                throw new WebSocketException("Couldn't connect to web socket");
            }
            
            State = WebSocketState.Open;
        }
        //sends to websocket serealized request as json | if it is first coin starts receive method 
        public async Task SubscribeCoinStreamAsync(Tradeble coin,string queryWebSocket)
        {
            try
            {
                _tradebles.Add(coin);
                if (_tradebles.Count() == 1)
                {
                    var a = Task.Run(() => ReceiveStreamMessageAsync(), _tokenStream);
                }
                var request = new WebSocketRequest();
                request.param.Add(queryWebSocket);
                request.id = _id;
                var stringText = JsonConvert.SerializeObject(request);
                request = null;
                await _client.SendAsync(Encoding.UTF8.GetBytes(stringText), WebSocketMessageType.Text, true, _tokenStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        // sends to websocket serealized request as json and unsubscribes from coin data | if it was last coin disposes client 
        public async Task UnSubscribeCoinStreamAsync(Tradeble coin)
        {
            _sem.WaitOne();
            try
            {
                var request = new WebSocketRequest();
                request.method = "UNSUBSCRIBE";
                request.param.Add(coin.Name);
                var stringText = JsonConvert.SerializeObject(request);
                request = null;
                await _client.SendAsync(Encoding.UTF8.GetBytes(stringText), WebSocketMessageType.Text, true, _tokenStream);
                _tradebles.Remove(coin);
                if (_tradebles.Count == 0)
                {
                    _cancellationTokenSource.Cancel();
                    State = WebSocketState.Closed;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            _sem.Release();
        }
        // while cansellation is not requested async receives data from stream then serializes object 
        private async Task ReceiveStreamMessageAsync()
        {
            while (!_tokenStream.IsCancellationRequested)
            {
                _sem.WaitOne();
                try
                {
                    WebSocketReceiveResult result;
                    var buf = new byte[300];
                    await _client.ReceiveAsync(buf, _tokenStream);
                    await Task.Run(() => {
                        var resultString = Encoding.UTF8.GetString(buf);
                        var resultObject = JsonConvert.DeserializeObject<MiniTicker>(resultString);
                        if (resultObject.EventType != null)
                        {
                            var coin = _tradebles.FirstOrDefault(x => x.Name.ToLower() == resultObject.Symbol.ToLower());
                            if (coin != null)
                                coin.PushToCoinStream(resultObject);
                        }
                        else Console.WriteLine(resultString);
                    });
                }
                catch (TaskCanceledException ex)
                {
                    Console.WriteLine("Task was canseled");
                }
                _sem.Release();
            }
            _client.Dispose();
            State = WebSocketState.Closed;
        }
    }
}