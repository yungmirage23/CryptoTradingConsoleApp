TradingHost host = new TradingHost();

await host.ReadCommand();
//class TradingController
//{
//    List<ITradebl> Coens;
//    public TradingController(List<ITradebl> _coens)
//    {
//        Coens = _coens;
//    }
//    public void SubscribeToStreams()
//    {
//        foreach(var coen in Coens)
//        {
//            ConsoleEx.Log($"{coen.Name} stream subscribed");
//        }
//    }
//}
//interface ITradebl
//{
//    public string Name { get; set; }
//    public void MakeSnapshot();
//    public void StartStrategy();
//}
//class Coen:ITradebl
//{
//    public IStratagy Stratagy;
//    public string Name { get; set; }
//    public string Method { get; set; }
//    List<KlineAPI> coens;
//    public Coen(string _name,string _method,IStratagy _stratagy)
//    {
//        Name = _name;
//        Method = _method;
//        Stratagy = _stratagy;
//    }
//    public void MakeSnapshot()
//    {
//        ConsoleEx.Log($"Coin:{Name} Strategy:{Stratagy.Name} snapshot made");
//    }
//    public void StartStrategy()
//    {
//        Stratagy.LoadStrategy();
//    }

//}
//interface IStratagy
//{
//    public string Name { get; }
//    public void LoadStrategy();
//}
//class LevelsStratagy : IStratagy
//{
//    public string Name{ get { return "LevelsStrategy"; } }
//    public void LoadStrategy()
//    {
//        ConsoleEx.Log("LevelsStrategyStarted");
//    }
//}
//class FastTrade : IStratagy
//{
//    public string Name { get { return "FastTrade"; } }
//    public void LoadStrategy()
//    {
//        ConsoleEx.Log("FastStrategyStarted");
//    }
//}
