using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;

namespace ThreadLearning.Th.线程池
{
    public class ThreadPool2
    {
        static void AsyncOperation(object state)
        {
            WriteLine($"Operation State:{state ?? "NULL"}");
            WriteLine($"worker threadId:{CurrentThread.ManagedThreadId}");
            Sleep(TimeSpan.FromSeconds(2));
        }

        public static void Perform()
        {
            const int x = 1, y = 2;
            const string lambdaState = "lambda state 2";

            ThreadPool.QueueUserWorkItem(AsyncOperation);
            Sleep(TimeSpan.FromSeconds(2));

            ThreadPool.QueueUserWorkItem(AsyncOperation, "async state");
            Sleep(TimeSpan.FromSeconds(2));

            ThreadPool.QueueUserWorkItem(state =>
            {
                WriteLine($"Operation state:{state}");
                WriteLine($"worker thread id:{CurrentThread.ManagedThreadId}");
                Sleep(TimeSpan.FromSeconds(2));
            }, "lambda state");

            ThreadPool.QueueUserWorkItem(_ =>
            {
                WriteLine($"Opearation state:{x + y},{lambdaState}");
                WriteLine($"worker thread id:{CurrentThread.ManagedThreadId}");
                Sleep(TimeSpan.FromSeconds(2));
            }, "lambda state");

            Sleep(TimeSpan.FromSeconds(2));
        }
    }
}
