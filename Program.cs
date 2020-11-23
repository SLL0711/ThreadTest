using System;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;
using ThreadLearning.Th.设置线程优先级;
using ThreadLearning.Th.线程安全;
using ThreadLearning.Th.线程池;
using ThreadLearning.Th.任务并行;

namespace ThreadLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 前后台线程演示

            //ThreadLearning.Th.前后台线程.ThreadSample.MainTest.Main2();

            #endregion

            #region 线程安全

            //线程安全
            //var c = new Counter();

            //Thread t1 = new Thread(ThreadSafe.TestCount);
            //Thread t2 = new Thread(ThreadSafe.TestCount);
            //Thread t3 = new Thread(ThreadSafe.TestCount);

            //t1.Start(c);
            //t2.Start(c);
            //t3.Start(c);

            //t1.Join();
            //t2.Join();
            //t3.Join();

            //WriteLine(c.Count);

            //var c2 = new CounterWithLock();

            //t1 = new Thread(ThreadSafe.TestCount);
            //t2 = new Thread(ThreadSafe.TestCount);
            //t3 = new Thread(ThreadSafe.TestCount);

            //t1.Start(c2);
            //t2.Start(c2);
            //t3.Start(c2);

            //t1.Join();
            //t2.Join();
            //t3.Join();

            //WriteLine(c2.Count);

            //var c2 = new CounterWithNolock();

            //Thread t1 = new Thread(ThreadSafe.TestCount);
            //Thread t2 = new Thread(ThreadSafe.TestCount);
            //Thread t3 = new Thread(ThreadSafe.TestCount);

            //t1.Start(c2);
            //t2.Start(c2);
            //t3.Start(c2);

            //t1.Join();
            //t2.Join();
            //t3.Join();

            //WriteLine(c2.Count);

            #endregion

            #region 死锁

            //object lock1 = new object(), lock2 = new object();

            ////锁lock1
            //new Thread(() => Deadlock.LockTooMuch(lock1, lock2)).Start();

            //lock (lock2)
            //{
            //    Sleep(TimeSpan.FromSeconds(1));
            //    if (Monitor.TryEnter(lock1, TimeSpan.FromSeconds(5)))
            //    {
            //        //获取到lock1锁控制权
            //        WriteLine("哈哈 拿到锁了");
            //    }
            //    else
            //    {
            //        WriteLine("呜呜,获取锁控制权失败");
            //    }
            //}

            //WriteLine("-----------------分界线---------------------");

            ////锁lock1
            //new Thread(() => Deadlock.LockTooMuch(lock1, lock2)).Start();
            //lock (lock2)
            //{
            //    Sleep(TimeSpan.FromSeconds(1));
            //    lock (lock1)
            //    {
            //        WriteLine("没有出现死锁,哈哈哈哈");
            //    }
            //}

            #endregion

            #region Mutex实现多应用程序间的线程同步

            //new MytexTest().Mtest();
            //new MytexTest().MtestCreateMore();

            #endregion

            #region SemaphoreSlim实现线程同步 设置至多并发线程数

            //for (int i = 0; i < 6; i++)
            //{
            //    string threadName = $"Thread_{i}";
            //    new Thread((j) =>
            //    {
            //        SemaphoreSlimTest.AccessDb(threadName, (int)j * 3 + 1);
            //    }).Start(i);
            //}

            #endregion

            #region 使用AutoSetEvent实现线程通信

            //new Thread(()=> {
            //    AutoSetEventTest.MainThreadPerform();
            //}).Start();

            #endregion

            #region ManualSetEventSlim类实现线程通信、混合模式更佳

            //ManualSetEventSlimtest.MainTask();

            #endregion

            #region 使用CountDownEvent实现等待多个线程执行完毕

            //CountDownEventTest.CountDown();

            #endregion

            #region 实现多线程的迭代同步执行

            //BarrierTest.BarrierPerfom();

            #endregion

            #region 使用读锁、写锁、升级锁实现线程安全

            //ReadWriteLockSlimTest.RwPerform();

            #endregion

            #region SpinWait

            //SpinWaitTest.SpinPerform();

            #endregion

            #region 线程池

            //ThreadPoolPerformDl.Perfom();
            //ThreadPool2.Perform();
            //ThreadPool3.Perform();
            //ThreadPool4.Perform();
            //ThreadPool5.Perform();
            //TimerTest.Perform();
            //BackgroundWorkerTest.Perform();

            #endregion

            #region 并行任务

            //Task1.Perform();
            //Task2.Perform();
            //Task3.Perform();
            //Task4.Perform();
            Task5.Perform();

            #endregion

            ReadLine();
        }
    }
}