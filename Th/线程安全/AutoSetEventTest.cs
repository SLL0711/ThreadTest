using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;

namespace ThreadLearning.Th.线程安全
{
    public class AutoSetEventTest
    {
        static AutoResetEvent _mainEvent = new AutoResetEvent(false);
        static AutoResetEvent _workEvent = new AutoResetEvent(false);

        /// <summary>
        /// 工作线程执行代码
        /// </summary>
        public static void WorkThreadPerform()
        {
            WriteLine("run for a long time task...");

            Thread.Sleep(10 * 1000);

            WriteLine("work done...");

            //状态切换到signal 通知主线程执行代码
            _workEvent.Set();

            WriteLine("wait for main thread to complete the work...");

            _mainEvent.WaitOne();

            WriteLine("start second operation...");

            Thread.Sleep(10 * 1000);

            WriteLine("complete");

            _workEvent.Set();
        }

        /// <summary>
        /// 主线程的执行代码
        /// </summary>
        public static void MainThreadPerform()
        {
            new Thread(() => { WorkThreadPerform(); }).Start();

            WriteLine("wait for work thread run ...");

            _workEvent.WaitOne();

            WriteLine("first operation is completed!");

            WriteLine("main task start...");

            Thread.Sleep(5 * 1000);

            _mainEvent.Set();

            WriteLine("now running the second operation on the second thread...");

            _workEvent.WaitOne();

            WriteLine("second opeation is run completed!");
        }
    }
}
