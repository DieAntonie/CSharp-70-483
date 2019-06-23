using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter1.ImplementingMultithreadingAsynchronousProcessing
{
    class UsingTasks
    {
        public static void Run()
        {
            Starting_A_New_Task();
            Using_Task_That_Returns_Value();
            Adding_Continuation();
            Different_Continuation_Options();
            Attaching_Child_To_Parent();
            Using_Task_Factory();
            Using_Task_WaitAll();
            Using_Task_WaitAny();
        }

        static void Starting_A_New_Task()
        {
            Task task = Task.Run(() =>
            {
                for (int x = 0; x < 100; x++)
                {
                    Console.Write('*');
                }

                Console.WriteLine();
            });

            task.Wait();
        }

        private static void Using_Task_That_Returns_Value()
        {
            Task<int> returningTask = Task.Run(() =>
            {
                return 42;
            });

            Console.WriteLine("Task Result: {0}", returningTask.Result);
        }

        private static void Adding_Continuation()
        {
            Task<int> continuedTask = Task.Run(() =>
            {
                return 42;
            }).ContinueWith((i) =>
            {
                return i.Result * 2;
            });

            Console.WriteLine("Continued Task Result: {0}", continuedTask.Result);
        }

        private static void Different_Continuation_Options()
        {
            Task<int> continuationTask = Task.Run(() =>
            {
                return 42;
            });

            continuationTask.ContinueWith((i) =>
            {
                Console.WriteLine("Canceled with {0}", i);
            }, TaskContinuationOptions.OnlyOnCanceled);

            continuationTask.ContinueWith((i) =>
            {
                Console.WriteLine("Faulted with {0}", i);
            }, TaskContinuationOptions.OnlyOnFaulted);

            var completedTask = continuationTask.ContinueWith((i) =>
            {
                Console.WriteLine("Completed with {0}", i.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completedTask.Wait();
        }

        private static void Attaching_Child_To_Parent()
        {
            Task<int[]> parent = Task.Run(() =>
            {
                var results = new int[3];
                new Task(() => results[0] = 0, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2, TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            var finalTask = parent.ContinueWith(parentTask =>
            {
                foreach (int item in parentTask.Result)
                {
                    Console.WriteLine("Child result {0}", item);
                }
            });

            finalTask.Wait();
        }

        private static void Using_Task_Factory()
        {
            Task<int[]> parent = Task.Run(() =>
            {
                var results = new int[3];

                TaskFactory taskFactory = new TaskFactory(
                    TaskCreationOptions.AttachedToParent,
                    TaskContinuationOptions.ExecuteSynchronously);
                taskFactory.StartNew(() => results[0] = 0);
                taskFactory.StartNew(() => results[1] = 1);
                taskFactory.StartNew(() => results[2] = 2);

                return results;
            });

            var finalTask = parent.ContinueWith(parentTask =>
            {
                foreach (int item in parentTask.Result)
                {
                    Console.WriteLine("Factory Child result: {0}", item);
                }
            });

            finalTask.Wait();
        }

        private static void Using_Task_WaitAll()
        {
            Task[] allTaskList = new Task[3];

            allTaskList[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("All Task 1");
                return 1;
            });

            allTaskList[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("All Task 2");
                return 2;
            });

            allTaskList[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("All Task 3");
                return 3;
            });

            Task.WaitAll(allTaskList);
        }

        private static void Using_Task_WaitAny()
        {
            Task<int>[] anyTaskList = new Task<int>[3];

            anyTaskList[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Any Task 1");
                return 1;
            });

            anyTaskList[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Any Task 2");
                return 2;
            });

            anyTaskList[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Any Task 3");
                return 3;
            });

            while (anyTaskList.Length > 0)
            {
                int index = Task.WaitAny(anyTaskList);
                Task<int> completedTask = anyTaskList[index];

                Console.WriteLine("Removing any task with result {0}", completedTask.Result);

                var temp = anyTaskList.ToList();
                temp.RemoveAt(index);
                anyTaskList = temp.ToArray();
            }
        }
    }
}
