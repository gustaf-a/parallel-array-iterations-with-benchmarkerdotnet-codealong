This repository contains the project I made when coding along this excellent blog post:

- [How to Beat Array Iteration Performance with Parallelism in C# .NET](https://michaelscodingspot.com/array-iteration-vs-parallelism-in-c-net/)

# Current features:

- Benchmarking of different kinds of parallel for-loops using [BenchmarkerDotNet](https://benchmarkdotnet.org/)

Example-results N = {1000, 10 000, 100 000}

|                      Method |      N |          Mean |         Error |        StdDev |
|---------------------------- |------- |--------------:|--------------:|--------------:|
|         **SequentialIteration** |   **1000** |      **1.631 us** |     **0.0046 us** |     **0.0043 us** |
|          ThreadPoolWithLock |   1000 |     87.333 us |     1.6934 us |     2.6365 us |
|   ThreadPoolWithInterlocked |   1000 |    309.661 us |     6.1810 us |    12.4860 us |
|                 ParallelFor |   1000 |    161.628 us |     3.2282 us |     5.4816 us |
| ParallelForWithLocalFinally |   1000 |      4.102 us |     0.0207 us |     0.0194 us |
|         **SequentialIteration** |  **10000** |     **16.481 us** |     **0.0457 us** |     **0.0427 us** |
|          ThreadPoolWithLock |  10000 |    689.432 us |    16.7437 us |    33.8231 us |
|   ThreadPoolWithInterlocked |  10000 |    917.826 us |     9.0719 us |     7.5754 us |
|                 ParallelFor |  10000 |  2,167.551 us |    87.9643 us |   259.3647 us |
| ParallelForWithLocalFinally |  10000 |      8.039 us |     0.1549 us |     0.1449 us |
|         **SequentialIteration** | **100000** |    **164.777 us** |     **0.5870 us** |     **0.5491 us** |
|          ThreadPoolWithLock | 100000 |  3,902.931 us |    24.0021 us |    22.4515 us |
|   ThreadPoolWithInterlocked | 100000 | 20,054.282 us |   517.7788 us | 1,485.6044 us |
|                 ParallelFor | 100000 | 20,369.558 us | 1,917.6523 us | 5,654.2410 us |
| ParallelForWithLocalFinally | 100000 |     34.615 us |     0.6832 us |     1.0637 us |