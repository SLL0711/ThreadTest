using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;

namespace ThreadLearning.Th.线程安全
{
    public class CountDownEventTest
    {
        static CountdownEvent countdownEvent = new CountdownEvent(2);//需要两次signal信号才会取消阻塞

        static void PerformOperator(string message, int seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));

            WriteLine(message);

            countdownEvent.Signal();
        }

        public static void CountDown()
        {
            WriteLine("start two operator");

            var t1 = new Thread(() => { PerformOperator("Operator 1 is Completed", 4); });
            var t2 = new Thread(() => { PerformOperator("Operator 2 is Completed", 8); });

            t1.Start();
            t2.Start();

            countdownEvent.Wait();
            WriteLine("Both Operator have been completed");
            countdownEvent.Dispose();
        }
    }
}
