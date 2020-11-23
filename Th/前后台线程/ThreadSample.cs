using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.前后台线程
{
    public class ThreadSample
    {
        private int _interations;
        public ThreadSample(int interations)
        {
            _interations = interations;
        }

        public void CountNumber()
        {
            for (int i = 0; i < _interations; i++)
            {
                Sleep(TimeSpan.FromSeconds(0.5));
                WriteLine($"{CurrentThread.Name} prints {i}");
            }
        }

        public class MainTest
        {
            public static void Main2()
            {
                ThreadSample ts1 = new ThreadSample(10);
                ThreadSample ts2 = new ThreadSample(20);

                Thread t1 = new Thread(ts1.CountNumber);
                t1.Name = "ForegroundThread";

                Thread t2 = new Thread(ts2.CountNumber);
                t2.Name = "BackgroundThread";
                t2.IsBackground = true;

                t1.Start();
                t2.Start();
            }
        }
    }
}
