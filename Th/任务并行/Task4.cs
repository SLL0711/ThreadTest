using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;
using System.Threading.Tasks;

namespace ThreadLearning.Th.任务并行
{
    delegate string AsynchronousTask(string threadName);
    delegate string IncompatibleAsynchronousTask(out int threadId);

    public class Task4
    {
        static void Callback(IAsyncResult ar)
        {
            WriteLine("Starting a callback...");
            WriteLine($"State passed to a callback:{ ar.AsyncState}");
            WriteLine($"Is threadpool thread:{CurrentThread.IsThreadPoolThread}");
            WriteLine($"Threadpool worker thread id:{CurrentThread.ManagedThreadId}");
        }

        static string Test(string threadName)
        {
            WriteLine("Starting...");
            WriteLine($"Is threadpool thread:{CurrentThread.IsThreadPoolThread}");
            Sleep(TimeSpan.FromSeconds(2));
            CurrentThread.Name = threadName;
            return $"Thread Name:{CurrentThread.Name}";
        }

        static string Test(out int ThreadId)
        {
            WriteLine("Starting...");
            WriteLine($"Is threadpool thread:{CurrentThread.IsThreadPoolThread}");
            Sleep(TimeSpan.FromSeconds(2));
            ThreadId = CurrentThread.ManagedThreadId;
            return $"Thread id:{ThreadId}";
        }

        //TODO:Not Support this method
        public static void Perform()
        {
            int threadId;
            AsynchronousTask d = Test;
            IncompatibleAsynchronousTask e = Test;

            WriteLine("Option 1");
            Task<string> task = Task<string>.Factory.FromAsync(d.BeginInvoke("AsyncTaskThread"
                , Callback, "a delegate async call"), d.EndInvoke);
            task.ContinueWith(t =>
            {
                WriteLine($"Callback is finished,now running a continuation! Result:{t.Result}");
            });

            while (!task.IsCompleted)
            {
                WriteLine(task.Status);
                Sleep(TimeSpan.FromSeconds(0.5));
            }
            WriteLine(task.Status);
            Sleep(TimeSpan.FromSeconds(1));

            WriteLine("---------------------------------------------------------------------");
            WriteLine();
            WriteLine("Option 2");

            task = Task<string>.Factory.FromAsync(d.BeginInvoke, d.EndInvoke, "AsyncTaskThread", "a delegate async call.");

            task.ContinueWith(t =>
            {
                WriteLine($"Task is completed,now running a continuation! Result:{t.Result}");
            });

            while (!task.IsCompleted)
            {
                WriteLine(task.Result);
                Sleep(TimeSpan.FromSeconds(0.5));
            }

            WriteLine("---------------------------------------------------------------------");
            WriteLine();
            WriteLine("Option 3");

            IAsyncResult ar = e.BeginInvoke(out threadId, Callback, "a delegate async call.");
            task = Task<string>.Factory.FromAsync(ar, _ =>
            {
                return e.EndInvoke(out threadId, ar);
            });

            task.ContinueWith(t =>
            {
                WriteLine($"Task is completed,now running a continuation! Result:{t.Result},ThreadId:{threadId}");
            });

            while (!task.IsCompleted)
            {
                WriteLine(task.Result);
                Sleep(TimeSpan.FromSeconds(0.5));
            }
            WriteLine(task.Status);

            Sleep(TimeSpan.FromSeconds(1));
        }
    }
}
