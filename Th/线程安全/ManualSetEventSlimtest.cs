using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;

namespace ThreadLearning.Th.线程安全
{
    public class ManualSetEventSlimtest
    {
        static ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(false);

        public static void WorkTask(int seconds,string threadName)
        {
            WriteLine($"{threadName} fall to sleep ...");

            Thread.Sleep(TimeSpan.FromSeconds(seconds));

            WriteLine($"{threadName} is wait for gate open...");

            manualResetEventSlim.Wait();

            WriteLine($"{threadName} enter the gate");
        }

        public static void MainTask()
        {
            new Thread(()=> { WorkTask(5,"thread1"); }).Start();
            new Thread(() => { WorkTask(6, "thread2"); }).Start();
            new Thread(() => { WorkTask(12, "thread3"); }).Start();

            WriteLine("main task is running...");

            Thread.Sleep(8 * 1000);

            manualResetEventSlim.Set();

            WriteLine("the gate are opened...");

            Thread.Sleep(TimeSpan.FromSeconds(2));

            WriteLine("the gate is closed");

            manualResetEventSlim.Reset();

            Thread.Sleep(TimeSpan.FromSeconds(6));

            WriteLine("the gate open second");

            manualResetEventSlim.Set();
        }
    }
}
