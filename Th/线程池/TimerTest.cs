using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.线程池
{
    public class TimerTest
    {
        static Timer _timer;

        static void TimerOperation(DateTime start)
        {
            TimeSpan timeSpan = DateTime.Now - start;
            WriteLine($"{timeSpan.Seconds} seconds from {start}. Timer thread pool thread id:{CurrentThread.ManagedThreadId} is in thread pool:{CurrentThread.IsThreadPoolThread}");
        }

        public static void Perform()
        {
            WriteLine("Press Enter to stop the timer...");
            DateTime start = DateTime.Now;
            _timer = new Timer(_ =>
            {
                TimerOperation(start);
            }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            try
            {
                Sleep(TimeSpan.FromSeconds(6));

                _timer.Change(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4));
                
                ReadLine();
            }
            finally
            {
                _timer.Dispose();
            }
        }
    }
}
