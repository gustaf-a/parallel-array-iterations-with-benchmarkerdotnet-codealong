This repository contains the project I made when coding along this excellent blog post:

- [How to Beat Array Iteration Performance with Parallelism in C# .NET](https://michaelscodingspot.com/array-iteration-vs-parallelism-in-c-net/)

# Current features:

- Benchmarking of different kinds of parallel for-loops using [BenchmarkerDotNet](https://benchmarkdotnet.org/)

Example-results N = {1000, 10 000, 100 000}

|                        Method |          Mean |         Error |        StdDev |        Median |
|------------------------------ |--------------:|--------------:|--------------:|--------------:|
|         SequentialIterationN0 |      2.098 us |     0.0417 us |     0.0542 us |      2.115 us |
|         SequentialIterationN1 |     16.629 us |     0.1629 us |     0.1444 us |     16.583 us |
|         SequentialIterationN2 |    166.562 us |     1.5695 us |     1.4682 us |    166.437 us |
|          ThreadPoolWithLockN0 |     80.543 us |     1.6111 us |     4.1298 us |     80.430 us |
|          ThreadPoolWithLockN1 |    674.277 us |     8.2550 us |     7.7217 us |    675.148 us |
|          ThreadPoolWithLockN2 |  4,004.129 us |    42.9179 us |    40.1455 us |  4,001.491 us |
|   ThreadPoolWithInterlockedN0 |    315.434 us |     6.1823 us |    11.1480 us |    314.001 us |
|   ThreadPoolWithInterlockedN1 |  2,418.441 us |    48.3543 us |   137.1730 us |  2,437.409 us |
|   ThreadPoolWithInterlockedN2 | 19,157.914 us | 1,265.4330 us | 3,671.2505 us | 20,163.371 us |
|                 ParallelForN0 |    164.308 us |     3.2823 us |     7.9271 us |    164.051 us |
|                 ParallelForN1 |  2,196.501 us |    76.7411 us |   226.2728 us |  2,182.177 us |
|                 ParallelForN2 | 18,535.350 us | 1,605.0137 us | 4,656.4358 us | 19,774.677 us |
| ParallelForWithLocalFinallyN0 |      4.065 us |     0.0180 us |     0.0168 us |      4.063 us |
| ParallelForWithLocalFinallyN1 |      8.005 us |     0.1157 us |     0.1083 us |      8.026 us |
| ParallelForWithLocalFinallyN2 |     34.506 us |     0.7452 us |     0.9425 us |     34.404 us |
