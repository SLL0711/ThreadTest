using System.Threading.Tasks;
using static System.Threading.Thread;
using static System.Console;
using System;

namespace ThreadLearning.Th.任务并行
{
    public class Task13
    {
        public async static void Perform()
        {
            Task t = AsyncProcessing();
            t.Wait();
        }

        static async Task AsyncProcessing()
        {
            WriteLine("1. Single exception;");

            try
            {
                string result = await GetInfoAsync("Task 1", 2);
                WriteLine(result);
            }
            catch (Exception ex)
            {
                WriteLine($"Exception details:{ex}");
            }

            WriteLine();
            WriteLine("2. Multiple Exception");

            Task<string> t1 = GetInfoAsync("Task 1", 3);
            Task<string> t2 = GetInfoAsync("Task 2", 2);
            try
            {
                string[] results = await Task.WhenAll(t1, t2);
                WriteLine(results.Length);
            }
            catch (Exception ex)
            {
                WriteLine($"Exception details:{ex}");
            }

            WriteLine();
            WriteLine("3. Multiple exceptions with AggregateException");
            t1 = GetInfoAsync("Task 1", 3);
            t2 = GetInfoAsync("Task 2", 2);
            Task<string[]> t3 = Task.WhenAll(t1, t2);
            try
            {
                string[] results = await t3;
                WriteLine(results.Length);
            }
            catch (Exception ex)
            {
                var ae = t3.Exception.Flatten();
                var exceptions = ae.InnerExceptions;
                WriteLine($"Exceptions caught :{exceptions.Count}");
                foreach (var e in exceptions)
                {
                    WriteLine($"Exception details:{e}");
                    WriteLine();
                }

                Console.WriteLine(t3.Exception);
            }

            WriteLine();
            WriteLine("4. await in catch and finally blocks.");

            try
            {
                string result = await GetInfoAsync("Task 1", 2);
                WriteLine(result);
            }
            catch (Exception ex)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                WriteLine($"Catch block with await:Exception details:{ex}");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                WriteLine("Finally block");
            }
        }

        static async Task<string> GetInfoAsync(string name, int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            throw new Exception($"Boom from {name}");
        }
    }
}
