using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;
using System.Linq;

namespace ThreadLearning.Th.PLINQ
{
    public class ParallerT
    {
        public static void Perform()
        {
            Parallel.Invoke(
                () => EmulateProcessing("Task 1"),
                () => EmulateProcessing("Task 2"),
                () => EmulateProcessing("Task 3"));

            var cts = new CancellationTokenSource();
            var result = Parallel.ForEach(
                Enumerable.Range(1, 30),
                new ParallelOptions
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    TaskScheduler = TaskScheduler.Default
                },
                (i, state) =>
                {
                    WriteLine(i);
                //    WriteLine($"task_{i} was processed on a thread id {CurrentThread.ManagedThreadId} " +
                //$"Is in threadpool:{CurrentThread.IsThreadPoolThread}");
                    if (i ==20)
                    {
                        state.Break();
                        WriteLine($"Loop is stopped:{state.IsStopped}");
                    }
                });

            WriteLine("------");
            WriteLine($"Iscompleted:{result.IsCompleted}");
            WriteLine($"Lowest break iteration:{result.LowestBreakIteration}");
        }

        static string EmulateProcessing(string taskName)
        {
            Sleep(TimeSpan.FromMilliseconds(
                new Random(DateTime.Now.Millisecond).Next(250, 350)));
            WriteLine($"{taskName} task was processed on a thread id {CurrentThread.ManagedThreadId} " +
                $"Is in threadpool:{CurrentThread.IsThreadPoolThread}");
            return taskName;
        }
    }
}
