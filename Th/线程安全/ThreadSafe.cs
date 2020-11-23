using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadLearning.Th.线程安全
{
    public class ThreadSafe
    {
        public static void TestCount(object counter)
        {

            for (int i = 0; i < 10000; i++)
            {
                ((CounterBase)counter).Increment();
                ((CounterBase)counter).Decrement();
            }
        }
    }

    public abstract class CounterBase
    {
        protected int _count;
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }

        public abstract void Decrement();

        public abstract void Increment();
    }

    public class Counter : CounterBase
    {
        public override void Decrement()
        {
            Count--;
        }

        public override void Increment()
        {
            Count++;
        }
    }

    public class CounterWithLock : CounterBase
    {
        private object _object = new object();

        public override void Decrement()
        {
            lock (_object)
            {
                Count--;
            }
        }

        public override void Increment()
        {
            lock (_object)
            {
                Count++;
            }
        }
    }

    public class CounterWithNolock : CounterBase
    {
        public override void Decrement()
        {
            Interlocked.Decrement(ref _count);
        }

        public override void Increment()
        {
            Interlocked.Increment(ref _count);
        }
    }
}
