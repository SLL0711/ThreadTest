using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.任务并行
{
    public class Task1
    {
        static void TestMethod(string name)
        {
            WriteLine($"Task {name} is running on a thread id {CurrentThread.ManagedThreadId} Is threadpool thread:{CurrentThread.IsThreadPoolThread}");
        }

        public static void Perform()
        {
            var t1 = new Task(() => { TestMethod("Task 1"); });
            var t2 = new Task(() => { TestMethod("Task 2"); });

            t1.Start();
            t2.Start();

            Task.Run(() => { TestMethod("Task 3"); });

            Task.Factory.StartNew(() => { TestMethod("Task 4"); });
            Task.Factory.StartNew(() => { TestMethod("Task 5"); }, TaskCreationOptions.LongRunning);//长时间运行线程使用单独线程

            Sleep(TimeSpan.FromSeconds(1));
        }
    }
}
