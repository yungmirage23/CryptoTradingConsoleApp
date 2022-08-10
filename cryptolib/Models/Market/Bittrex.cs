using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.CryptoMarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptolib.Models.Market
{
    public class Bittrex
    {
        public readonly string Name="Bittrex";
        public List<Tradeble> Tradebles = new List<Tradeble>();

    }
}
