using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Cryptodll.Models;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.CryptoMarket;
using cryptolib.Models.Binance;
using System;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Tradeble btcusdt = new Coin("btcusdt");
        CryptoMarket binance = new Binance();

        await binance.ConnectToMarketAsync(MarketEnum.Spot);
        binance.SubscribeToCoinDataAsync(btcusdt, MarketEnum.Spot);
        //await binance.AccountData();


        Console.ReadLine();
    }
}

public static class UnitTest
{
    public static void TestMethod1()
    {
        BenchmarkRunner.Run<ParalVsSimple>();
    }
}

public class ParalVsSimple
{
    public ParalVsSimple()
    {
    }

    [Benchmark(Description = "P")]
    public void FindLowHighParalel()
    {
    }

    [Benchmark(Description = "S")]
    public void FindLowHigh()
    {
    }
}