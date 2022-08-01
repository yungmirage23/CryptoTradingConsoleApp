using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Cryptodll.Models;
using Cryptodll.Models.Cryptocurrency;
using Cryptodll.Models.CryptoMarket;
using cryptolib.Models.Binance;
Tradeble btcusdt = new Coin("btcusdt");
Tradeble ethusdt = new Coin("ethusdt");
Tradeble ethusdtspot = new Coin("btcusdt");
Tradeble btcusdtspot = new Coin("ethusdt");
CryptoMarket binance = new Binance();
try
{
    await binance.ConnectToMarketAsync(MarketEnum.Futures);
    await binance.ConnectToMarketAsync(MarketEnum.Spot);
    binance.SubscribeToCoinDataAsync(btcusdt, MarketEnum.Futures);
    binance.SubscribeToCoinDataAsync(btcusdtspot, MarketEnum.Spot);
    binance.SubscribeToCoinDataAsync(ethusdt, MarketEnum.Futures);
    binance.SubscribeToCoinDataAsync(ethusdtspot, MarketEnum.Spot);

}
catch(Exception ex)
{
    ConsoleEx.Log(ex);
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















