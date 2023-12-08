``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 10 (10.0.19045.3693)
12th Gen Intel Core i7-12800H, 1 CPU, 20 logical and 14 physical cores
.NET SDK=8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
|                                     Method |       Mean |     Error |    StdDev |    Gen0 |   Gen1 | Allocated |
|------------------------------------------- |-----------:|----------:|----------:|--------:|-------:|----------:|
|                          &#39;int - List cons&#39; |   2.375 μs | 0.0473 μs | 0.1059 μs |  2.5482 | 0.4234 |   32000 B |
|                 &#39;int - ImmutableList cons&#39; |  95.410 μs | 1.7462 μs | 1.6334 μs | 40.0391 | 9.6436 |  502896 B |
|                       &#39;int - List.reverse&#39; |   2.511 μs | 0.0413 μs | 0.0606 μs |  2.5482 | 0.4234 |   32000 B |
|              &#39;int - ImmutableList.reverse&#39; |  71.121 μs | 0.6854 μs | 0.6411 μs |  3.7842 | 0.8545 |   48024 B |
|                           &#39;int - List.map&#39; |   2.781 μs | 0.0543 μs | 0.0687 μs |  2.5482 | 0.5074 |   32000 B |
|   &#39;int - ImmutableList map by LINQ Select&#39; |  31.375 μs | 0.5986 μs | 0.7571 μs |  4.1504 | 0.9766 |   52200 B |
|       &#39;int - ImmutableList map by SetItem&#39; | 113.180 μs | 2.1415 μs | 2.4661 μs | 36.2549 |      - |  455376 B |
|       &#39;int - ImmutableList map by Builder&#39; |  36.315 μs | 0.6762 μs | 0.6944 μs |  3.7842 | 1.0376 |   48072 B |
|                        &#39;int - List.filter&#39; |   1.756 μs | 0.0350 μs | 0.0623 μs |  1.2741 | 0.1411 |   16000 B |
| &#39;int - ImmutableList filter by LINQ Where&#39; |  13.979 μs | 0.2794 μs | 0.3825 μs |  2.2736 | 0.2747 |   28672 B |
|  &#39;int - ImmutableList filter by RemoveAll&#39; |  57.953 μs | 0.9039 μs | 0.8455 μs |  2.3804 | 0.2441 |   30376 B |
|                        &#39;int - List.reduce&#39; |   1.095 μs | 0.0148 μs | 0.0138 μs |       - |      - |         - |
|               &#39;int - ImmutableList.reduce&#39; |   4.495 μs | 0.0656 μs | 0.0806 μs |  0.0076 |      - |     112 B |
|                      &#39;int - List.contains&#39; |   5.087 μs | 0.0649 μs | 0.0607 μs |       - |      - |      40 B |
|             &#39;int - ImmutableList.contains&#39; |  12.743 μs | 0.1634 μs | 0.1448 μs |       - |      - |      72 B |
