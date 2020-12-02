using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task9
    {
        public static void Perform()
        {
            Task t = AsyncWithTPL();
            t.Wait();

            t = AsyncWait();
            t.Wait();
        }

        static Task AsyncWithTPL()
        {
            Task<string> t = GetInfoAsync("Task 1");
            Task t2 = t.ContinueWith(task => WriteLine(t.Result), TaskContinuationOptions.NotOnFaulted);
            Task t3 = t.ContinueWith(task => WriteLine(t.Exception.InnerException), TaskContinuationOptions.OnlyOnFaulted);

            return Task.WhenAny(t2, t3);
        }

        static async Task AsyncWait()
        {
            try
            {
                string result = await GetInfoAsync("task 2");
                WriteLine(result);
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }
        }

        static async Task<string> GetInfoAsync(string name)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            return $"Task {name} is running on a thread id {CurrentThread.ManagedThreadId} " +
                $"is threadpool thread:{CurrentThread.IsThreadPoolThread}";
        }
    }
}
