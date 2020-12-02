using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task14
    {
        static async Task<string> GetInfoAsync(string name, int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            if (name.Contains("Exception"))
            {
                throw new Exception($"Boom from {name}");
            }
            var str = $"Task {name} is running on a thread id {CurrentThread.ManagedThreadId} " +
                $"Is threadpool thread:{CurrentThread.IsThreadPoolThread}";
            return str;
        }

        static async Task AsyncTaskWithErrors()
        {
            string result = await GetInfoAsync("AsyncTaskException", 2);
            WriteLine(result);
        }

        static async void AsyncVoidWithErrors()
        {
            string result = await GetInfoAsync("AsyncVoidException", 2);
            WriteLine(result);
        }

        static async Task AsyncTask()
        {
            string result = await GetInfoAsync("AsyncTask", 2);
            WriteLine(result);
        }

        static async void AsyncVoid()
        {
            string result = await GetInfoAsync("AsyncVoid", 2);
            WriteLine(result);
        }

        public static void Perform()
        {
            AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs arg) =>
            {

            };

            Task t = AsyncTask();
            t.Wait();

            AsyncVoid();
            Sleep(TimeSpan.FromSeconds(3));

            t = AsyncTaskWithErrors();
            while (!t.IsFaulted)
            {
                Sleep(TimeSpan.FromSeconds(1));
            }
            WriteLine(t.Exception);

            try
            {
                AsyncVoidWithErrors();
                Sleep(TimeSpan.FromSeconds(3));
            }
            catch (Exception ex)
            {
                WriteLine(ex);
            }

            int[] numbers = { 1, 2, 3, 4, 5 };
            Array.ForEach(numbers, async number =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                if (number == 3) throw new Exception("Boom!");
                WriteLine(number);
            });
        }

        //static async void ThrowExceptionFromVoid()
        //{
        //    throw new Exception("ThrowExceptionFromVoid");
        //}

        //static async Task ThrowExceptionFromTask()
        //{
        //    var a = CurrentThread.ManagedThreadId + "," + CurrentThread.IsThreadPoolThread;
        //    throw new Exception("ThrowExceptionFromTask");
        //}
    }
}
