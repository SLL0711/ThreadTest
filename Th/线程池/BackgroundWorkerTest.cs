using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;

namespace ThreadLearning.Th.线程池
{
    public class BackgroundWorkerTest
    {
        static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            WriteLine($"DoWork thread pool thread id:{CurrentThread.ManagedThreadId}");
            var bw = (BackgroundWorker)sender;
            for (int i = 1; i <= 100; i++)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (i % 10 == 0)
                {
                    bw.ReportProgress(i);
                }

                Sleep(TimeSpan.FromSeconds(0.1));
            }

            e.Result = 42;
        }

        static void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WriteLine($"{e.ProgressPercentage}% Completed. Progress thread poll thread id:{CurrentThread.ManagedThreadId}");
        }

        static void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            WriteLine($"Competed thread pool thread id:{CurrentThread.ManagedThreadId}");
            if (e.Error != null)
            {
                WriteLine($"Exception {e.Error.Message} has occured.");
            }
            else if (e.Cancelled)
            {
                WriteLine("Operation has been canceled.");
            }
            else
            {
                WriteLine($"The answer is:{e.Result}");
            }
        }

        public static void Perform()
        {
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            bw.DoWork += Worker_DoWork;
            bw.RunWorkerCompleted += Worker_Completed;
            bw.ProgressChanged += Worker_ProgressChanged;

            bw.RunWorkerAsync();

            WriteLine("Press C to cancel work.");
            do
            {
                if (ReadKey().KeyChar == 'C')
                {
                    bw.CancelAsync();
                }
            } while (bw.IsBusy);
        }
    }
}
