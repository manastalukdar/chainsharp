using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace chainsharp.scale
{
    static class Program
    {

        static void Main(string[] args)
        {
            var engine = new Engine();

            TimeSpan TimeSpanForRun = TimeSpan.FromMinutes(Constants.TimeToRunInMinutes);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while(true)
            {
                engine.WriteTaskAsync().GetAwaiter().GetResult();
                if (stopWatch.Elapsed > TimeSpanForRun)
                {
                    break;
                }
            }

            stopWatch.Stop();

            stopWatch.Start();
            while (true)
            {
                engine.ReadTaskAsync().GetAwaiter().GetResult();
                if (stopWatch.Elapsed > TimeSpanForRun)
                {
                    break;
                }
            }

            stopWatch.Stop();

            engine.Dispose();
            Console.WriteLine("Hit any key to exit");
            Console.ReadLine();
        }
    }
}
