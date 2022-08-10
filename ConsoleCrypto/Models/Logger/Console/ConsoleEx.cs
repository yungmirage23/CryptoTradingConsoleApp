using System;
using System.Threading;

static class ConsoleEx
{
    
    public static void Log(object? message)
    {
        var TimeNow = DateTime.Now.ToLongTimeString();
        Console.WriteLine($"[{TimeNow}] |Thread: {Thread.CurrentThread.ManagedThreadId} | {message.ToString()}");
    }
    public static void Log()
    {
        var TimeNow = DateTime.Now.ToLongTimeString();
        Console.WriteLine($"[{TimeNow}] |Thread: {Thread.CurrentThread.ManagedThreadId} |");
    }
}




