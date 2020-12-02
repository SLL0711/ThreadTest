using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task12
    {
        static async Task AsyncProcessing()
        {
            Task<string> t1 = GetInfoAsync("Task 1", 3);
            Task<string> t2 = GetInfoAsync("Task 2", 10);

            string[] results = await Task.WhenAll(t1, t2);
            foreach (var item in results)
            {
                WriteLine(item);
            }
        }

        static async Task<string> GetInfoAsync(string name, int seconds)
        {
            WriteLine($"running on a thread id { CurrentThread.ManagedThreadId}" +
                $"Is threadpool thread:{CurrentThread.IsThreadPoolThread}");

            //await Task.Delay(TimeSpan.FromSeconds(seconds)).ConfigureAwait(false);

            await Task.Run(()=> {
                Sleep(TimeSpan.FromSeconds(seconds));
            }) ;

            //await Task.Run(()=> {
            //    Sleep(TimeSpan.FromSeconds(seconds));
            //});

            WriteLine($"running on a thread id { CurrentThread.ManagedThreadId}" +
                $"Is threadpool thread:{CurrentThread.IsThreadPoolThread}");

            return $"Task {name} is running on a thread id {CurrentThread.ManagedThreadId} " +
                $"Is threadpool thread:{CurrentThread.IsThreadPoolThread}";
        }

        public static void Perform()
        {
            Task t = AsyncProcessing();
            t.Wait();
        }
    }
}
