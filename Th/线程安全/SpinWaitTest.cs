using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.线程安全
{
    public class SpinWaitTest
    {
        static volatile bool _isCompleted = false;

        static void UserModelWait()
        {
            while (!_isCompleted)
            {
                WriteLine(".");
            }

            WriteLine();
            WriteLine("waiting is complete");
        }

        static void HybridWait()
        {
            var w = new SpinWait();
            while (!_isCompleted)
            {
                w.SpinOnce();

                WriteLine(w.NextSpinWillYield);
            }
        }

        public static void SpinPerform()
        {
            var t1 = new Thread(UserModelWait);
            var t2 = new Thread(HybridWait);

            WriteLine("running user model waiting");
            t1.Start();
            Sleep(TimeSpan.FromSeconds(20));
            _isCompleted = true;
            Sleep(TimeSpan.FromSeconds(1));
            _isCompleted = false;
            WriteLine("running hybrid spinwair construct waiting");
            t2.Start();
            Sleep(TimeSpan.FromSeconds(20));
            _isCompleted = true;
        }
    }
}
