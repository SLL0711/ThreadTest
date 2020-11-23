using System;
using System.Collections.Generic;
using System.Text;
using static System.Threading.Thread;

namespace ThreadLearning.Th.线程安全
{
    public class Deadlock
    {
        public static void LockTooMuch(object lock1, object lock2)
        {
            lock (lock1)
            {
                Sleep(TimeSpan.FromSeconds(1));
                lock (lock2) { };
            }
        }
    }
}
