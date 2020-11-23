using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task3
    {
        static int TestMethod(string name, int seconds)
        {
            WriteLine($"Task {name} is running on a thread id {CurrentThread.ManagedThreadId} Is threadpool thread:{CurrentThread.IsThreadPoolThread}");
            Sleep(TimeSpan.FromSeconds(seconds));
            return 42 * seconds;
        }

        public static void Perform()
        {
            var firstTask = new Task<int>(() => TestMethod("first task", 3));
            var secondTask = new Task<int>(() => TestMethod("second task", 2));

            firstTask.ContinueWith(t =>
            {
                WriteLine($"The first answer is {t.Result}. Thread id {CurrentThread.ManagedThreadId} is in threadpool:{CurrentThread.IsThreadPoolThread}");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            firstTask.Start();
            secondTask.Start();

            Sleep(TimeSpan.FromSeconds(4));

            Task continuation = secondTask.ContinueWith(t =>
            {
                WriteLine($"The second answer is {t.Result}. Thread id {CurrentThread.ManagedThreadId} is in threadpool:{CurrentThread.IsThreadPoolThread}");
            }, TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously);

            continuation.GetAwaiter().OnCompleted(() =>
            {
                WriteLine($"Continuation task is completed. Thread id {CurrentThread.ManagedThreadId} is in threadpool:{CurrentThread.IsThreadPoolThread}");
            });

            Sleep(TimeSpan.FromSeconds(2));
            WriteLine();

            firstTask = new Task<int>(() =>
            {
                var innerTask = Task.Factory.StartNew(() =>
                {
                    TestMethod("second task", 5);
                    WriteLine("second task is completed. then perform third task.");
                }, TaskCreationOptions.AttachedToParent);

                innerTask.ContinueWith(t => TestMethod("third task", 2), TaskContinuationOptions.AttachedToParent);

                return TestMethod("First task", 2);
            });

            firstTask.Start();

            while (!firstTask.IsCompleted)
            {
                WriteLine(firstTask.Status);
                Sleep(TimeSpan.FromSeconds(0.5));
            }

            Sleep(TimeSpan.FromSeconds(10));
        }
    }
}
