﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptodll.Models.Cryptocurrency
{
    public class Coin: Tradeble
    {
        public string Name;
        public Coin(string name) : base(name) {
            Name = name;
        }

        Stack<MiniTicker> tickerQueue=new Stack<MiniTicker>();
        IEnumerable<KlineAPI> coinKlines;
        
    }
}


//https://api.binance.com/api/v3/klines?symbol=BTCUSDT&interval=1m
// wss://stream.binance.com:9443/ws/BTCUSDT@aggTrade