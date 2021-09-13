using System;
using System.Diagnostics;
using Vkloud.Sync;


namespace Vkloud
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            var t1 = new LocalFileStorage(@"C:\Users\ivche\Documents\vkloud");
            var t3 = new RemoteFileStorage("", "");
            var s = new Synchronizer(t1, t3);
            s.Init().Wait();

            Console.ReadKey();
        }
    }
}
