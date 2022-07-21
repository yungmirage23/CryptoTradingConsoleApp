using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Models.Cryptocurrency
{
    public class Coin: ITradeble
    {
        public string Name;
        public string Method;
        string ITradeble.Method { get => this.Method; }
        string ITradeble.Name { get => this.Name; }
        public Coin(string coinTicker,string method)
        {
            Name = coinTicker.ToLower();
            Method = method;
        }
        Stack<MiniTicker> tickerQueue=new Stack<MiniTicker>();
        Stack<MiniTicker> ITradeble.tickerStreamsQueue => this.tickerQueue;

        IEnumerable<KlineAPI> ITradeble.coinKlines { get => this.coinKlines; set => coinKlines= value;  }

        IEnumerable<KlineAPI> coinKlines;



        public void MakeCoinSnapshot()
        {
            throw new NotImplementedException();
        }
        public void ConnectToCoinStream()
        {
            throw new NotImplementedException();
        }
    }
}


//https://api.binance.com/api/v3/klines?symbol=BTCUSDT&interval=1m
// wss://stream.binance.com:9443/ws/BTCUSDT@aggTrade