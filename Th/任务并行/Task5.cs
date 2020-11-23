using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;
using System.ComponentModel;

namespace ThreadLearning.Th.任务并行
{
    public class Task5
    {
        static int TaskMethod(string name, int seconds)
        {
            WriteLine($"Task {name} is running on a thread id:{CurrentThread.ManagedThreadId}. is in threadpool thread :{CurrentThread.IsThreadPoolThread}");

            Sleep(TimeSpan.FromSeconds(seconds));
            return 42 * seconds;
        }

        public static void Perform()
        {
            try
            {
                var tcs = new TaskCompletionSource<int>();

                var worker = new BackgroundWorker();
                worker.DoWork += (sender, eventArgs) =>
                {
                    eventArgs.Result = TaskMethod("background worker", 5);
                };

                worker.RunWorkerCompleted += (sender, eventArgs) =>
                {
                    if (eventArgs.Error != null)
                    {
                        tcs.TrySetException(eventArgs.Error);
                    }
                    else if (eventArgs.Cancelled)
                    {
                        tcs.TrySetCanceled();
                    }
                    else
                    {
                        tcs.TrySetResult((int)eventArgs.Result);
                    }
                };

                worker.RunWorkerAsync();

                int result = tcs.Task.Result;

                WriteLine($"Result is:{result}");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
