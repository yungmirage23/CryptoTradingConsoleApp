using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using ConsoleCrypto.Services.Trading;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.Market;
using Cryptodll.Models.Strategy;

var coin = new Coin("BTCUSDT", "@miniTicker");


var cryptoMarket = new CryptoMarket("Binance", Market.Futures);
var controller = new TradingController(cryptoMarket);

await controller.StartReceivingCoin(coin);

await controller.StartTrading(coin,new LevelsStrategy());







Console.ReadLine();







//public class ParalVsSimple
//{
//    Tradeble coin = new Coin("BTCUSDT", "@miniTicker");

//    [Benchmark(Description ="Paralel")]
//    public async Task TestParalelAsync()
//    {
//        var options = new ParallelOptions { MaxDegreeOfParallelism = 10 };
//        await Parallel.ForEachAsync(coin.timeframes, options, async (tf, token) =>
//        {
//            Thread.Sleep(10);
//            ConsoleEx.Log($"API {coin.Name} data for {tf} delivered");
//        });
//    }
//    [Benchmark(Description = "Simple")]
//    public async Task Test()
//    {
//      foreach(var tf in coin.timeframes)
//        {
//            Thread.Sleep(10);
//            ConsoleEx.Log($"API {coin.Name} data for {tf} delivered");
//        }
//    }

//}

//public static class UnitTest
//{
//    public static void TestMethod1()
//    {
//        BenchmarkRunner.Run<ParalVsSimple>();
//    }
//}
