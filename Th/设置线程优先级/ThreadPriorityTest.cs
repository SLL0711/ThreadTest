using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;
using static System.Diagnostics.Process;

namespace ThreadLearning.Th.设置线程优先级
{
    public class ThreadPriorityTest
    {
        public static void ThreadRun()
        {
            ThreadSample sample = new ThreadSample();

            Thread t1 = new Thread(sample.CountNumber);
            Thread t2 = new Thread(sample.CountNumber);

            t1.Priority = ThreadPriority.Highest;
            t2.Priority = ThreadPriority.Lowest;

            t1.Name = "ThreadOne";
            t2.Name = "ThreadTwo";

            t1.Start();
            t2.Start();

            Sleep(TimeSpan.FromSeconds(2));
            sample.Stop();
        }

        public static void Main2()
        {
            WriteLine(CurrentThread.Priority);

            ThreadPriorityTest.ThreadRun();

            Sleep(TimeSpan.FromSeconds(2));

            WriteLine("单核运行...");

            //设置当前进程中所有线程被运行在内核1上
            GetCurrentProcess().ProcessorAffinity = new IntPtr(1);
            ThreadPriorityTest.ThreadRun();
        }

        class ThreadSample
        {
            private bool _isStop = false;

            public void Stop()
            {
                if (!_isStop)
                {
                    _isStop = true;
                }
            }

            public void CountNumber()
            {
                long count = 0;

                while (!_isStop)
                {
                    count++;
                }

                WriteLine($"{CurrentThread.Name} with {CurrentThread.Priority} has a count {count}");
            }
        }
    }
}
