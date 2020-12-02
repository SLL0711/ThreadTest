using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task11
    {
        public static void Perform()
        {
            Task t = AsyncWithTPL();
            t.Wait();

            t = AsyncWithAwait();
            t.Wait();
        }

        static Task AsyncWithTPL()
        {
            var continurationTask = new Task(() =>
            {
                Task<string> t = GetInfoAsync("TPL 1");

                t.ContinueWith(task =>
                {
                    WriteLine(t.Result);
                    Task<string> t2 = GetInfoAsync("TPL 2");
                    t2.ContinueWith(innerTask =>
                    {
                        WriteLine(innerTask.Result);
                    }, TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.AttachedToParent);
                    t2.ContinueWith(innerTask =>
                    {
                        WriteLine(innerTask.Exception.InnerException);
                    }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);
                }, TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.AttachedToParent);

                t.ContinueWith(task =>
                {
                    WriteLine(t.Exception.InnerException);
                }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);
            });

            continurationTask.Start();
            return continurationTask;
        }

        static async Task AsyncWithAwait()
        {
            try
            {
                string result = await GetInfoAsync("Async 1");
                WriteLine(CurrentThread.ManagedThreadId);
                WriteLine(result);
                result = await GetInfoAsync("Async 2");
                WriteLine(result);
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }
        }

        static async Task<string> GetInfoAsync(string name)
        {
            WriteLine($"Task {name} started!");
            await Task.Delay(TimeSpan.FromSeconds(5));
            if (name == "TPL 2")
                throw new Exception("Boom!");

            var s = $"Task {name} is running on a thread id {CurrentThread.ManagedThreadId} " +
                $"Is threadpool thread :{CurrentThread.IsThreadPoolThread}";
            WriteLine(s);
            return s;
        }
    }
}
