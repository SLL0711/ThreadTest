using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.线程安全
{
    public class ReadWriteLockSlimTest
    {
        static ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
        static Dictionary<int, int> dic = new Dictionary<int, int>();

        static void Write(string theadName)
        {
            while (true)
            {
                try
                {
                    int newKey = new Random().Next(250);
                    lockSlim.EnterUpgradeableReadLock();//开启升级锁模式，可与读锁并存
                    if (!dic.ContainsKey(newKey))
                    {
                        try
                        {
                            lockSlim.EnterWriteLock();//开启写锁模式
                            dic[newKey] = 1;
                            WriteLine($"New key {newKey} is added to the dictionary by th {theadName}");
                        }
                        finally
                        {
                            lockSlim.ExitWriteLock();
                        }

                        Sleep(TimeSpan.FromSeconds(0.1));
                    }
                }
                finally
                {
                    lockSlim.ExitUpgradeableReadLock();
                }
            }
        }

        static void Read()
        {
            WriteLine("reading contents of dictionary");
            while (true)
            {
                try
                {
                    lockSlim.EnterReadLock();
                    foreach (var key in dic.Keys)
                    {
                        Sleep(TimeSpan.FromSeconds(0.1));
                    }
                }
                finally
                {
                    lockSlim.ExitReadLock();
                }
            }
        }

        public static void RwPerform()
        {
            new Thread(() => { Read(); }) { IsBackground = true }.Start();
            new Thread(() => { Read(); }) { IsBackground = true }.Start();
            new Thread(() => { Read(); }) { IsBackground = true }.Start();

            new Thread(() => { Write("Thread 1"); }) { IsBackground = true }.Start();
            new Thread(() => { Write("Thread 2"); }) { IsBackground = true }.Start();

            Sleep(TimeSpan.FromSeconds(30));
        }
    }
}
