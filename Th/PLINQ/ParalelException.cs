using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace ThreadLearning.Th.PLINQ
{
    public class ParalelException
    {
        public static void Perform()
        {
            IEnumerable<int> numbers = Enumerable.Range(-5, 10);
            var query = from n in numbers
                        select 100 / n;
            try
            {
                foreach (var n in query)
                {
                    WriteLine(n);
                }
            }
            catch (Exception)
            {
                WriteLine("Divied by zero!");
            }

            WriteLine("---");
            WriteLine("Sequential LINQ query processing.");
            WriteLine();

            var paralelQuery = from n in numbers.AsParallel()
                               select 100 / n;
            try
            {
                paralelQuery.ForAll(WriteLine);
                //foreach (var n in paralelQuery)
                //{
                //    WriteLine(n);
                //}
            }
            catch (DivideByZeroException e)
            {
                WriteLine("divided by zero - usual exception handler.");
            }
            catch (AggregateException e)
            {
                e.Flatten().Handle(ex =>
                {
                    if (ex is DivideByZeroException)
                    {
                        WriteLine("Divided by zero - aggreagate exception handler.");
                        return true;
                    }
                    return false;
                });
            }
        }
    }
}
