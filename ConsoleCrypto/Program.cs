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

public static class UnitTest
{
    public static void TestMethod1()
    {
        BenchmarkRunner.Run<ParalVsSimple>();
    }
}

public class ParalVsSimple
{
    private Tradeble _coin = new Coin("BTCUSDT", "@miniTicker");
    private TradingController controller;
    private LevelsStrategy strategy = new LevelsStrategy();
    private int _coefficient = 10;

    public ParalVsSimple()
    {
    }

    [Benchmark(Description = "Paralel")]
    public void FindLowHighParalel()
    {
        
    }

    [Benchmark(Description = "Simple")]
    public void FindLowHigh()
    {

    }
}