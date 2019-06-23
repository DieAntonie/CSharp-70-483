using System;
using System.Threading;

namespace Chapter1.ImplementingMultithreadingAsynchronousProcessing
{
    class UnderstandingThreads
    {
        public static bool Stopped;

        [ThreadStatic]
        public static int _field;

        public static ThreadLocal<int> _record = new ThreadLocal<int>(() =>
        {
            return Thread.CurrentThread.ManagedThreadId;
        });

        public static void Run()
        {
            Thread background_thread = new Thread(new ThreadStart(ThreadMethod));
            background_thread.IsBackground = true;
            background_thread.Start();

            Thread parameterized_thread = new Thread(new ParameterizedThreadStart(ThreadMethod));
            parameterized_thread.Start(10);

            Stopped = false;
            Thread interupted_thread = new Thread(new ThreadStart(InteruptMethod));
            interupted_thread.Start();

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Hello World!");
                Thread.Sleep(0);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            Stopped = true;

            ThreadPool.QueueUserWorkItem((s) =>
            {
                Console.WriteLine("Working on Thread {0} from Thread pool", _record);
            });

            new Thread(() =>
            {
                Console.WriteLine("ThreadLocal A: {0}", _record);
            }).Start();

            new Thread(() =>
            {
                Console.WriteLine("ThreadLocal B: {0}", _record);
            }).Start();

            parameterized_thread.Join();
            interupted_thread.Join();

            Console.ReadLine();
        }

        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                _field++;
                Console.WriteLine("ThreadProc: {0}", _field);
                Thread.Sleep(0);
            }
        }

        public static void ThreadMethod(object int_obj)
        {
            for (int i = 0; i < (int)int_obj; i++)
            {
                _field++;
                Console.WriteLine("ThreadProc: {0} of {1}", _field, (int)int_obj);
                Thread.Sleep(0);
            }
        }

        public static void InteruptMethod()
        {
            while (!Stopped)
            {
                Console.WriteLine("Running...");
                Thread.Sleep(0);
            }
        }
    }
}
