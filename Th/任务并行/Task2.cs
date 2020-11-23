using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task2
    {
        static Task<int> CreateTask(string name)
        {
            return new Task<int>(() => TestMethod(name));
        }

        static int TestMethod(string name)
        {
            WriteLine($"Task {name} is running on a thread id {CurrentThread.ManagedThreadId} Is threadpool thread:{CurrentThread.IsThreadPoolThread}");
            Sleep(TimeSpan.FromSeconds(2));
            return 42;
        }

        public static void Perform()
        {
            TestMethod("main thread task");//同步运行

            Task<int> task = CreateTask("Task 1");
            task.Start();
            int result = task.Result;
            WriteLine($"Result is {result}");

            task = CreateTask("Task 2");
            task.RunSynchronously();
            result = task.Result;
            WriteLine($"Result is {result}");

            task = CreateTask("task 3");
            WriteLine(task.Status);
            task.Start();
            while (!task.IsCompleted)
            {
                WriteLine(task.Status);
                Sleep(TimeSpan.FromSeconds(0.5));
            }

            WriteLine(task.Status);
            result = task.Result;
            WriteLine($"Result is :{result}");
        }
    }
}
