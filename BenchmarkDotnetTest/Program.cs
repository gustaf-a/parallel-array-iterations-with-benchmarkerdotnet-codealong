using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchmarkDotNetTest
{

    public class ParallelTesting
    {
        private int[] N = { 1000, 10000, 100000 };
        private int ITEMS;
        private readonly int[] arr = null;
        private int partSize;

        private object _lock = new object();

        public ParallelTesting()
        {
            int maxItems = N[2];
            arr = new int[maxItems];
            var rnd = new Random();
            for (int i = 0; i < maxItems; i++)
            {
                arr[i] = rnd.Next(1000);
            }

            partSize = maxItems / 4;
        }

        [Benchmark]
        public long SequentialIterationN0()
        {
            ITEMS = N[0];
            var result =  SequentialIteration();
            return result;
        }
        [Benchmark]
        public long SequentialIterationN1()
        {
            ITEMS = N[1];
            return SequentialIteration();
        }
        [Benchmark]
        public long SequentialIterationN2()
        {
            ITEMS = N[2];
            return SequentialIteration();
        }

        public long SequentialIteration()
        {
            long total = 0;
            for (int i = 0; i < ITEMS; i++)
            {
                total += arr[i];
            }

            return total;
        }


        [Benchmark]
        public long ThreadPoolWithLockN0()
        {
            ITEMS = N[0];
            return ThreadPoolWithLock();
        }
        [Benchmark]
        public long ThreadPoolWithLockN1()
        {
            ITEMS = N[1];
            return ThreadPoolWithLock();
        }
        [Benchmark]
        public long ThreadPoolWithLockN2()
        {
            ITEMS = N[2];
            return ThreadPoolWithLock();
        }
        
        public long ThreadPoolWithLock()
        {
            long total = 0;
            int threads = 4;
            partSize = ITEMS / threads;
            Task[] tasks = new Task[threads];
            for (int iThread = 0; iThread < threads; iThread++)
            {
                var localThread = iThread;
                tasks[localThread] = Task.Run(() =>
                {
                    for (int j = localThread * partSize; j < (localThread + 1) * partSize; j++)
                    {
                        lock (_lock)
                        {
                            total += arr[j];
                        }
                    }
                });
            }
            Task.WaitAll(tasks);
            return total;
        }

        [Benchmark]
        public long ThreadPoolWithInterlockedN0()
        {
            ITEMS = N[0];
            return ThreadPoolWithInterlocked();
        }
        [Benchmark]
        public long ThreadPoolWithInterlockedN1()
        {
            ITEMS = N[1];
            return ThreadPoolWithInterlocked();
        }
        [Benchmark]
        public long ThreadPoolWithInterlockedN2()
        {
            ITEMS = N[2];
            return ThreadPoolWithInterlocked();
        }

        public long ThreadPoolWithInterlocked()
        {
            long total = 0;
            int threads = 4;
            partSize = ITEMS / threads;
            Task[] tasks = new Task[threads];
            for (int iThread = 0; iThread < threads; iThread++)
            {
                var localThread = iThread;
                tasks[localThread] = Task.Run(() =>
                {
                    for (int j = localThread * partSize; j < (localThread + 1) * partSize; j++)
                    {
                        lock (_lock)
                        {
                            Interlocked.Add(ref total, arr[j]);
                        }
                    }
                });
            }
            Task.WaitAll(tasks);
            return total;
        }

        [Benchmark]
        public long ParallelForN0()
        {
            ITEMS = N[0];
            return ParallelFor();
        }
        [Benchmark]
        public long ParallelForN1()
        {
            ITEMS = N[1];
            return ParallelFor();
        }
        [Benchmark]
        public long ParallelForN2()
        {
            ITEMS = N[2];
            return ParallelFor();
        }

        public long ParallelFor()
        {
            long total = 0;
            int parts = 4;
            int partSize = ITEMS / parts;
            var parallel = Parallel.For(0, parts, new ParallelOptions(), (iter) =>
            {
                for (int j = iter * partSize; j < (iter + 1) * partSize; j++)
                {
                    Interlocked.Add(ref total, arr[j]);
                }
            });
            return total;
        }

        [Benchmark]
        public int ParallelForWithLocalFinallyN0()
        {
            ITEMS = N[0];
            return ParallelForWithLocalFinally();
        }

        [Benchmark]
        public int ParallelForWithLocalFinallyN1()
        {
            ITEMS = N[1];
            return ParallelForWithLocalFinally();
        }

        [Benchmark]
        public int ParallelForWithLocalFinallyN2()
        {
            ITEMS = N[2];
            return ParallelForWithLocalFinally();
        }

        public int ParallelForWithLocalFinally()
        {
            int total = 0;
            int parts = 4;
            int partSize = ITEMS / parts;
            var parallel = Parallel.For(0, parts,
                localInit: () => 0,
                body: (iterations, state, localTotal) =>
                 {
                     for (int j = iterations * partSize; j < (iterations + 1) * partSize; j++)
                     {
                         localTotal += arr[j];
                     }
                     return localTotal;
                 },
                localFinally:(localTotal) =>
                {
                    total += localTotal;
                });
                return total;
        }
    }



    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ParallelTesting>();
            Console.ReadKey();
        }
    }
}