using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace ThreadLearning.Th.并发集合
{
    public class ConcurrentStack
    {
        public static void Perform()
        {
            Task t = RunProgram();
            t.Wait();
        }

        static async Task RunProgram()
        {
            var taskQueue = new ConcurrentStack<CustomTask>();
            var cts = new CancellationTokenSource();

            var taskSource = Task.Run(() => { TaskProducer(taskQueue); });

            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processerid = i.ToString();
                processors[i - 1] = Task.Run(() =>
                {
                    TaskProcessor(taskQueue, $"processor:{processerid}", cts.Token);
                });
            }

            await taskSource;
            cts.CancelAfter(TimeSpan.FromSeconds(2));

            await Task.WhenAll(processors);
        }

        static async Task TaskProducer(ConcurrentStack<CustomTask> queue)
        {
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask { Id = i };
                queue.Push(workItem);
                WriteLine($"task {workItem.Id} has been posted.");
            }
        }

        static async Task TaskProcessor(ConcurrentStack<CustomTask> queue, string name, CancellationToken token)
        {
            CustomTask workItem;
            bool dequeueSuccessful = false;

            await GetRandomDelay();

            do
            {
                dequeueSuccessful = queue.TryPop(out workItem);
                if (dequeueSuccessful)
                {
                    WriteLine($"Task {workItem.Id} has been processed by {name}");
                }

                await GetRandomDelay();

            } while (!token.IsCancellationRequested);
        }

        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }

        class CustomTask
        {
            public int Id { get; set; }
        }
    }
}
