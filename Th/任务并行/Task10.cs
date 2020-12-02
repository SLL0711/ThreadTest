using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task10
    {
        static async Task AsyncProcessing()
        {
            Func<string, Task<string>> func = async name =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                var str = $"Task {name} is running on a thread id {CurrentThread.ManagedThreadId} " +
                $"Is threadpool thread :{CurrentThread.IsThreadPoolThread}";
                return str;
            };

            string result = await func("async lambda");

            WriteLine(result);
        }

        public static void Perform()
        {
            Task t = AsyncProcessing();
            t.Wait();
        }
    }
}
