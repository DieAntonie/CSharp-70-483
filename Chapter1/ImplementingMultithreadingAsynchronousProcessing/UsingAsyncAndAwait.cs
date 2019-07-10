using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter1.ImplementingMultithreadingAsynchronousProcessing
{
    class UsingAsyncAndAwait
    {
        public static void Run()
        {
            Run_Asynchronous_Method();
            Scalablity_Vs_Responsiveness();
            Using_Configure_Await();
        }

        private static void Run_Asynchronous_Method()
        {
            string result = _downloadContent().Result;
            Console.WriteLine($"Run_Asynchronous_Method: {result}");
        }

        private static async Task<string> _downloadContent()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://example.com/");
                return result;
            }
        }

        private static void Scalablity_Vs_Responsiveness()
        {
            var sleepA = _sleepAsyncA(1000);
            var sleepB = _sleepAsyncB(1000);

            Console.WriteLine($"_sleepAsyncA: {sleepA}");
            Console.WriteLine($"_sleepAsyncB: {sleepB}");
        }

        private static Task _sleepAsyncA(int millisecondTimeout)
        {
            return Task.Run(() => Thread.Sleep(millisecondTimeout));
        }

        private static Task _sleepAsyncB(int millisecondTimeout)
        {
            TaskCompletionSource<bool> taskCompletionSource = null;
            var timer = new Timer(delegate { taskCompletionSource.TrySetResult(true); }, null, -1, -1);
            taskCompletionSource = new TaskCompletionSource<bool>(timer);
            timer.Change(millisecondTimeout, -1);
            return taskCompletionSource.Task;
        }

        private static async void Using_Configure_Await()
        {
            Console.WriteLine("Using_Configure_Await START");

            HttpClient httpClient = new HttpClient();
            string content = await httpClient.GetStringAsync("http://example.com/").ConfigureAwait(false);

            Console.WriteLine($"Using_Configure_Await {content}");
        }
    }
}
