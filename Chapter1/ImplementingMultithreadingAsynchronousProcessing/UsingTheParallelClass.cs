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
            UsingParallelFor();
            UsingParallelForeach();
            UsingParallelBreak();
        }

        private static void UsingParallelFor()
        {
            Parallel.For(0, 10, value =>
            {
                Console.WriteLine($"Parallel for loop: {value}");
                Thread.Sleep(1000);
            });
        }

        private static void UsingParallelForeach()
        {
            var numbers = Enumerable.Range(0, 10);
            Parallel.ForEach(numbers, value =>
            {
                Console.WriteLine($"Parallel foreach loop: {value}");
                Thread.Sleep(1000);
            });
        }

        private static void UsingParallelBreak()
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
