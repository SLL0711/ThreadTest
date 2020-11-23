using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadLearning.Th.线程安全
{
    public class SemaphoreSlimTest
    {
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(4);

        public static void AccessDb(string name, int seconds)
        {
            Console.WriteLine($"{name} is connecting DB...");

            semaphoreSlim.Wait();

            Console.WriteLine($"{name} has granted to access the db");

            Thread.Sleep(TimeSpan.FromSeconds(seconds));

            semaphoreSlim.Release();
        }
    }
}
