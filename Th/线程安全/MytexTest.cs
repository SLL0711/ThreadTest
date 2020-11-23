using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;

namespace ThreadLearning.Th.线程安全
{
    public class MytexTest
    {
        private const string MUTEXTNAME = "CSHARPMUTEX";
        public void Mtest()
        {
            using (var mutex = new Mutex(true, MUTEXTNAME))
            {
                if (!mutex.WaitOne(TimeSpan.FromSeconds(5), true))
                {
                    WriteLine("Second Thread is Running");
                }
                else
                {
                    WriteLine("Running");
                    ReadLine();
                    mutex.ReleaseMutex();
                }
            }
        }

        public void MtestCreateMore()
        {
            bool flag;
            using (var mutex = new Mutex(true, MUTEXTNAME,out flag))
            {
                ReadLine();

                if (flag)
                {
                    WriteLine("create success");
                }
                else
                {
                    WriteLine("create false");
                }
            }
        }
    }
}
