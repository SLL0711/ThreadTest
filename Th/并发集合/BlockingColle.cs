using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ThreadLearning.Th.并发集合
{
    public class BlockingColle
    {
        public static void Perform()
        {
            WriteLine("Using a Queue inside of BlockingCollection");
            WriteLine();
            Task t = RunProgram();
            t.Wait();
            WriteLine();
            WriteLine("Using a Stack inside of BlockingCollection");
            WriteLine();
            t = RunProgram(new ConcurrentStack<CustomTask>());
            t.Wait();
        }

        static async Task RunProgram(IProducerConsumerCollection<CustomTask> collection = null)
        {
            var taskCollection = new BlockingCollection<CustomTask>();
            if (collection != null)
                taskCollection = new BlockingCollection<CustomTask>(collection);

            var taskSource = Task.Run(() => TaskProducer(taskCollection));

            Task[] processors = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string processorid = $"Processor {i}";
                processors[i - 1] = Task.Run(() => TaskProcessor(taskCollection, processorid));
            }

            await taskSource;

            await Task.WhenAll(processors);
        }

        static async Task TaskProducer(BlockingCollection<CustomTask> collection)
        {
            for (int i = 1; i <= 20; i++)
            {
                await Task.Delay(20);
                var workItem = new CustomTask { Id = i };
                collection.Add(workItem);
                WriteLine($"Task {workItem.Id} has been posted.");
            }

            collection.CompleteAdding();
        }

        static async Task TaskProcessor(BlockingCollection<CustomTask> collection, string name)
        {
            await GetRandomDelay();
            foreach (CustomTask item in collection.GetConsumingEnumerable())
            {
                WriteLine($"Task {item.Id} has been peocessed by {name}");
                await GetRandomDelay();
            }
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
