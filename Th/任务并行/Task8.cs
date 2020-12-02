using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task8
    {
        static int TaskMethod(string name, int seconds)
        {
            WriteLine($"Task {name} is running on a thread id:{CurrentThread.ManagedThreadId} Is threadpool thread:{CurrentThread.IsAlive}");
            Sleep(TimeSpan.FromSeconds(seconds));
            return 42 * seconds;
        }

        public static void Perform()
        {
            var firstTask = new Task<int>(() => TaskMethod("Task 1", 3));
            var secondTask = new Task<int>(() => TaskMethod("Task 2", 2));
            var whenAllTask = Task.WhenAll(firstTask, secondTask);

            whenAllTask.ContinueWith(t =>
            {
                WriteLine($"first answer is {t.Result[0]},the second answer is {t.Result[1]}");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            firstTask.Start();
            secondTask.Start();

            Sleep(TimeSpan.FromSeconds(4));

            var tasks = new List<Task<int>>();
            for (int i = 0; i < 4; i++)
            {
                int counter = i;
                var task = new Task<int>(() => TaskMethod($"Task {counter}", counter));
                tasks.Add(task);
                task.Start();
            }

            while (tasks.Count > 0)
            {
                var compolexTask = Task.WhenAny(tasks).Result;
                tasks.Remove(compolexTask);
                WriteLine($"a task has been completed with result {compolexTask.Result}");
            }

            Sleep(TimeSpan.FromSeconds(1));
        }
    }
}
