using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.线程池
{
    public class ThreadPool5
    {
        static void RunOperations(TimeSpan workerOperationTimespan)
        {
            using (var evt = new ManualResetEvent(false))
            {
                using (var cts = new CancellationTokenSource())
                {
                    WriteLine("Registering timeout opeation...");
                    var worker = ThreadPool.RegisterWaitForSingleObject(evt, (state, isTimeout) =>
                    {
                        WorkerOperationWait(cts, isTimeout);
                    }, null, workerOperationTimespan, true);

                    WriteLine("starting long running operation...");
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        WorkerOperation(cts.Token, evt);
                    });

                    Sleep(workerOperationTimespan.Add(TimeSpan.FromSeconds(2)));

                    worker.Unregister(evt);
                }
            }
        }

        static void WorkerOperation(CancellationToken token, ManualResetEvent evt)
        {
            for (int i = 0; i < 6; i++)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                Sleep(TimeSpan.FromSeconds(1));
            }

            evt.Set();
        }

        static void WorkerOperationWait(CancellationTokenSource cts, bool isTimeout)
        {
            if (isTimeout)
            {
                cts.Cancel();
                WriteLine("Worker Operation time out and was canceled");
            }
            else
            {
                WriteLine("Worker Operation succeded.");
            }
        }

        public static void Perform()
        {
            //RunOperations(TimeSpan.FromSeconds(5));
            RunOperations(TimeSpan.FromSeconds(7));
        }
    }
}
