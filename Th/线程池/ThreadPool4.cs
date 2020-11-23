using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.线程池
{
    class ThreadPool4
    {
        static void AsyncOperation1(CancellationToken token)
        {
            WriteLine("Starting the first task");
            for (int i = 0; i < 5; i++)
            {
                if (token.IsCancellationRequested)
                {
                    WriteLine("The first task has been cancled");
                    return;
                }

                Sleep(TimeSpan.FromSeconds(1));
            }

            WriteLine("The first task has completed successfully");
        }

        static void AsyncOperation2(CancellationToken token)
        {
            WriteLine("Starting the second task");
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Sleep(TimeSpan.FromSeconds(1));
                }

                WriteLine("The first task has completed successfully");
            }
            catch (OperationCanceledException ex)
            {
                WriteLine("The second task has ben cancled");
            }
        }

        static void AsyncOperation3(CancellationToken token)
        {
            bool cancellationFlag = false;
            token.Register(() =>
            {
                cancellationFlag = true;
            });

            WriteLine("Starting the third task");
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    if (cancellationFlag)
                    {
                        WriteLine("The third task has ben cancled");
                        return;
                    }
                    Sleep(TimeSpan.FromSeconds(1));
                }
            }
            catch (OperationCanceledException ex)
            {
                WriteLine("The second task has ben cancled");
            }

            WriteLine("The first task has completed successfully");
        }

        public static void Perform()
        {
            using (var cts = new CancellationTokenSource())
            {
                var tk = cts.Token;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    AsyncOperation1(tk);
                });
                Sleep(TimeSpan.FromSeconds(2));
                cts.Cancel();
            }

            using (var cts = new CancellationTokenSource())
            {
                var tk = cts.Token;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    AsyncOperation2(tk);
                });
                Sleep(TimeSpan.FromSeconds(2));
                cts.Cancel();
            }

            using (var cts = new CancellationTokenSource())
            {
                var tk = cts.Token;
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    AsyncOperation3(tk);
                });
                Sleep(TimeSpan.FromSeconds(2));
                cts.Cancel();
            }

            Sleep(TimeSpan.FromSeconds(10));
        }
    }
}
