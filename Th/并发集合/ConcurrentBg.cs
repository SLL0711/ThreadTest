using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ThreadLearning.Th.并发集合
{
    public class ConcurrentBg
    {
        static Dictionary<string, string[]> dic = new Dictionary<string, string[]>();

        public static async void Perform()
        {
            CreateLinks();

            var bag = new ConcurrentBag<CrawlingTask>();

            string[] urls = { "http://microsoft.com/","http://google.com/","http://facebook.com/","http://twitter.com"};

            var crawlers = new Task[4];
            for (int i = 1; i <= 4; i++)
            {
                string crawlerName = $"Crawler {i}";
                bag.Add(new CrawlingTask { UrlToCrawl = urls[i - 1], ProducerName = "root" });
                crawlers[i - 1] = Task.Run(() =>
                {
                    Crawl(bag, crawlerName);
                });
            }

            await Task.WhenAll(crawlers);
        }

        static async Task Crawl(ConcurrentBag<CrawlingTask> bag, string crawlerName)
        {
            CrawlingTask task;
            while (bag.TryTake(out task))
            {
                IEnumerable<string> urls = await GetLinksFromContent(task);
                if (urls != null)
                {
                    foreach (var url in urls)
                    {
                        var t = new CrawlingTask
                        {
                            UrlToCrawl = url,
                            ProducerName = crawlerName
                        };
                        bag.Add(t);
                    }
                }

                WriteLine($"indexing url {task.UrlToCrawl} posted by {task.ProducerName} is completed by {crawlerName}");
            }
        }

        static async Task<IEnumerable<string>> GetLinksFromContent(CrawlingTask task)
        {
            await GetRandomDelay();

            if (dic.ContainsKey(task.UrlToCrawl)) return dic[task.UrlToCrawl];

            return null;
        }

        static void CreateLinks()
        {
            dic["http://microsoft.com/"] = new[] { "http://microsoft.com/a.html", "http://microsoft.com/b.html" };
            dic["http://microsoft.com/a.html"] = new[] { "http://microsoft.com/c.html", "http://microsoft.com/d.html" };
            dic["http://microsoft.com/b.html"] = new[] { "http://microsoft.com/e.html", "http://microsoft.com/f.html" };


            dic["http://google.com/"] = new[] { "http://google.com/a.html", "http://google.com/b.html" };
            dic["http://google.com/a.html"] = new[] { "http://google.com/c.html", "http://google.com/d.html" };
            dic["http://google.com/b.html"] = new[] { "http://google.com/e.html", "http://google.com/f.html" };
            dic["http://google.com/c.html"] = new[] { "http://google.com/g.html", "http://google.com/h.html" };


            dic["http://facebook.com/"] = new[] { "http://facebook.com/a.html", "http://facebook.com/b.html" };
            dic["http://facebook.com/a.html"] = new[] { "http://facebook.com/c.html", "http://facebook.com/d.html" };
            dic["http://facebook.com/b.html"] = new[] { "http://facebook.com/e.html", "http://facebook.com/f.html" };


            dic["http://twitter.com"] = new[] { "http://twitter.com/a.html", "http://twitter.com/b.html" };
            dic["http://twitter.com/a.html"] = new[] { "http://twitter.com/c.html", "http://twitter.com/d.html" };
            dic["http://twitter.com/b.html"] = new[] { "http://twitter.com/e.html", "http://twitter.com/f.html" };
        }

        static Task GetRandomDelay()
        {
            int delay = new Random(DateTime.Now.Millisecond).Next(1, 500);
            return Task.Delay(delay);
        }

        class CrawlingTask
        {
            public string UrlToCrawl { get; set; }
            public string ProducerName { get; set; }
        }
    }
}
