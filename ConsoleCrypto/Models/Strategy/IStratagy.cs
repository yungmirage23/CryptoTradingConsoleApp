using ConsoleCrypto.Models.Cryptocurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCrypto.Models.Strategy
{
    internal interface IStrategy
    {
        public void StartStrategy(Tradeble coin);
    }
}
