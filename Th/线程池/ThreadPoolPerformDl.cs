using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace ThreadLearning.Th.线程池
{
    public delegate string RunOnThreadPool(out int threadId);

    public class ThreadPoolPerformDl
    {
        private static void CallBack(IAsyncResult ar)
        {
            WriteLine("Starting a callback");
            WriteLine($"State pass to a callback:{ar.AsyncState}");
            WriteLine($"is a pool thread:{CurrentThread.IsThreadPoolThread}");
            WriteLine($"Thread pool worker thread id:{CurrentThread.ManagedThreadId}");
        }

        private static string Test(out int threadId)
        {
            WriteLine("Starting");
            WriteLine($"is a pool thread:{CurrentThread.IsThreadPoolThread}");
            Sleep(TimeSpan.FromSeconds(2));
            threadId = CurrentThread.ManagedThreadId;
            return $"thread pool worker thread id was:{threadId}";
        }

        //TODO:unsupport platform
        public static void Perfom()
        {
            int threadId = 0;

            RunOnThreadPool poolDelegate = Test;

            var t = new Thread(() =>
            {
                Test(out threadId);
            });

            t.Start();
            t.Join();

            WriteLine($"Thread Id:{threadId}");

            IAsyncResult r = poolDelegate.BeginInvoke(out threadId, CallBack, "a delegate async call");

            string res = poolDelegate.EndInvoke(out threadId, r);

            WriteLine($"Thread pool worker thread Id:{threadId}");
            WriteLine(res);

            Sleep(TimeSpan.FromSeconds(2));
        }
    }
}
