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
        [Params(1000, 10000, 100000)]
        public int N { get; set; }

        private int[] arr = null;
        private int partSize;

        private object _lock = new object();

        [GlobalSetup]
        public void Setup()
        {
            arr = new int[N];
            var rnd = new Random();
            for (int i = 0; i < N; i++)
            {
                arr[i] = rnd.Next(1000);
            }

            partSize = N / 4;
        }

        [Benchmark]
        public long SequentialIteration()
        {
            long total = 0;
            for (int i = 0; i < N; i++)
            {
                total += arr[i];
            }

            return total;
        }


        [Benchmark]
        public long ThreadPoolWithLock()
        {
            long total = 0;
            int threads = 4;
            partSize = N / threads;
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
        public long ThreadPoolWithInterlocked()
        {
            long total = 0;
            int threads = 4;
            partSize = N / threads;
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
        public long ParallelFor()
        {
            long total = 0;
            int parts = 4;
            int partSize = N / parts;
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
        public int ParallelForWithLocalFinally()
        {
            int total = 0;
            int parts = 4;
            int partSize = N / parts;
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
                localFinally: (localTotal) =>
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