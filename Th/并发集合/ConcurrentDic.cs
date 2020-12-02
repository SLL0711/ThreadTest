using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Console;

namespace ThreadLearning.Th.并发集合
{
    public class ConcurrentDic
    {
        const string Item = "Dictionary Item";
        const int Interations = 1000000;
        public static string CurrentItem;

        public static void Perform()
        {
            var concurrentDictionary = new ConcurrentDictionary<int, string>();
            var dictionary = new Dictionary<int, string>();

            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < Interations; i++)
            {
                lock (dictionary)
                {
                    dictionary[i] = Item;
                }
            }
            sw.Stop();
            WriteLine($"Writing to dictionary with a lock:{sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < Interations; i++)
            {
                concurrentDictionary[i] = Item;
            }
            
            sw.Stop();
            WriteLine($"Writing to concurrentdictionary:{sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < Interations; i++)
            {
                lock (dictionary)
                {
                    CurrentItem = dictionary[i];
                }
            }
            sw.Stop();
            WriteLine($"Reading from dictionary with a lock:{sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < Interations; i++)
            {
                CurrentItem = concurrentDictionary[i];
            }
            sw.Stop();
            WriteLine($"Reading from the concurrentdictionary:{sw.Elapsed}");
        }
    }
}
