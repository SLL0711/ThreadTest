using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task15
    {
        public static void Perform()
        {
            Task t = AsyncProcessing();
            t.Wait();
        }

        public static async Task AsyncProcessing()
        {
            //var sync = new CustomAwaitable(true);
            //string result = await sync;
            //WriteLine(result);

            var async = new CustomAwaitable(false);
            string result = await async;

            WriteLine("1111");
            WriteLine(result);
        }
    }

    class CustomAwaitable
    {
        private readonly bool _completeSync;
        public CustomAwaitable(bool completeSync)
        {
            _completeSync = completeSync;
        }

        public CustomAwaiter GetAwaiter()
        {
            return new CustomAwaiter(_completeSync);
        }
    }

    class CustomAwaiter : INotifyCompletion
    {
        private string _result = "Completed sync";
        private readonly bool _completeSync;

        public bool IsCompleted => _completeSync;

        public CustomAwaiter(bool completeSync)
        {
            _completeSync = completeSync;
        }

        public string GetResult()
        {
            return _result;
        }

        public void OnCompleted(Action continuation)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                Sleep(TimeSpan.FromSeconds(1));
                _result = GetInfo();
                continuation?.Invoke();
            });
        }

        private string GetInfo()
        {
            var str = $"Task is running on a thread id{CurrentThread.ManagedThreadId}" +
                $" Is threadPool thread:{CurrentThread.IsThreadPoolThread}";
            return str;
        }
    }
}
