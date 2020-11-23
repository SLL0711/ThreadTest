using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
using static System.Threading.Thread;
using System.Threading;

namespace ThreadLearning.Th.线程安全
{
    public class BarrierTest
    {
        static Barrier barrier = new Barrier(2, (b) => { WriteLine($"End of phase {b.CurrentPhaseNumber}"); });
        static void PlayMusic(string name, string message, int seconds)
        {
            for (int i = 1; i < 3; i++)
            {
                WriteLine("----------------------------------");
                Sleep(TimeSpan.FromSeconds(seconds));
                WriteLine($"{name} starts to {message}");
                Sleep(TimeSpan.FromSeconds(seconds));
                WriteLine($"{name} finish to {message}");

                barrier.SignalAndWait();
            }
        }

        public static void BarrierPerfom()
        {
            var t1 = new Thread(() => { PlayMusic("thr guid", "play an amazing solo", 10); });
            var t2 = new Thread(() => { PlayMusic("thr singer", "sing his song", 1); });

            t1.Start();
            t2.Start();
        }
    }
}
