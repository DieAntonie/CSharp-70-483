using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter1.ImplementingMultithreadingAsynchronousProcessing
{
    class UsingTheParallelClass
    {
        public static void Run()
        {
            Using_Parallel_For();
            Using_Parallel_Foreach();
            Using_Parallel_Break();
        }

        private static void Using_Parallel_For()
        {
            Parallel.For(0, 10, value =>
            {
                Console.WriteLine($"Parallel for loop: {value}");
                Thread.Sleep(1000);
            });
        }

        private static void Using_Parallel_Foreach()
        {
            var numbers = Enumerable.Range(0, 10);
            Parallel.ForEach(numbers, value =>
            {
                Console.WriteLine($"Parallel foreach loop: {value}");
                Thread.Sleep(1000);
            });
        }

        private static void Using_Parallel_Break()
        {
            ParallelLoopResult result = Parallel.For(0, 10, (int value, ParallelLoopState loopState) =>
            {
                Console.WriteLine($"Parallel loop value: {value}; state: {loopState}");
                if (value == 5)
                {
                    Console.WriteLine("Breaking Loop");
                    loopState.Break();
                }
                return;
            });
        }
    }
}
