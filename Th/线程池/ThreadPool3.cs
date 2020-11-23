using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;
using System.Diagnostics;

namespace ThreadLearning.Th.线程池
{
    public class ThreadPool3
    {
        static void UseThreads(int count)
        {
            CountdownEvent countdownEvent = new CountdownEvent(count);
            for (int i = 0; i < count; i++)
            {
                var t = new Thread(() =>
                {
                    Write($"{CurrentThread.ManagedThreadId}");
                    Sleep(TimeSpan.FromSeconds(0.1));
                    countdownEvent.Signal();
                });
                t.Start();
            }

            countdownEvent.Wait();
            WriteLine();
            countdownEvent.Dispose();
        }

        static void UseThreadPool(int count)
        {
            CountdownEvent countdownEvent = new CountdownEvent(count);
            for (int i = 0; i < count; i++)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Write($"{CurrentThread.ManagedThreadId}");
                    Sleep(TimeSpan.FromSeconds(0.1));
                    countdownEvent.Signal();
                });
            }

            countdownEvent.Wait();
            WriteLine();
            countdownEvent.Dispose();
        }

        public static void Perform()
        {
            const int count = 500;

            var sw = new Stopwatch();
            sw.Start();
            UseThreads(count);
            sw.Stop();
            WriteLine($"execution time using threads:{sw.ElapsedMilliseconds}");

            sw.Reset();
            sw.Start();
            UseThreadPool(count);
            sw.Stop();
            WriteLine($"execution time using threadPool:{sw.ElapsedMilliseconds}");

            Sleep(TimeSpan.FromSeconds(10));
        }
    }
}
