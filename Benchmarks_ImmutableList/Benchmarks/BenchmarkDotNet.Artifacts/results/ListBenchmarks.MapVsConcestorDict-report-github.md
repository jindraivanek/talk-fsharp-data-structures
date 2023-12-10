``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.2715)
AMD Ryzen 7 5700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
|            Method | Size |            Mean |         Error |        StdDev |
|------------------ |----- |----------------:|--------------:|--------------:|
|           **MapFind** |   **64** |        **430.6 μs** |       **6.24 μs** |       **5.54 μs** |
| ConcestorDictFind |   64 |              NA |            NA |            NA |
|           **MapFind** |  **128** |      **2,037.8 μs** |      **30.77 μs** |      **28.78 μs** |
| ConcestorDictFind |  128 |              NA |            NA |            NA |
|           **MapFind** |  **256** |      **9,664.7 μs** |     **108.68 μs** |      **96.34 μs** |
| ConcestorDictFind |  256 |              NA |            NA |            NA |
|           **MapFind** |  **512** |     **45,931.0 μs** |     **892.71 μs** |   **1,028.04 μs** |
| ConcestorDictFind |  512 |              NA |            NA |            NA |
|           **MapFind** | **1024** |    **215,681.9 μs** |   **4,222.81 μs** |   **4,147.36 μs** |
| ConcestorDictFind | 1024 |              NA |            NA |            NA |
|           **MapFind** | **8192** | **21,629,045.3 μs** | **280,074.08 μs** | **248,278.37 μs** |
| ConcestorDictFind | 8192 |              NA |            NA |            NA |

Benchmarks with issues:
  MapVsConcestorDict.ConcestorDictFind: DefaultJob [Size=64]
  MapVsConcestorDict.ConcestorDictFind: DefaultJob [Size=128]
  MapVsConcestorDict.ConcestorDictFind: DefaultJob [Size=256]
  MapVsConcestorDict.ConcestorDictFind: DefaultJob [Size=512]
  MapVsConcestorDict.ConcestorDictFind: DefaultJob [Size=1024]
  MapVsConcestorDict.ConcestorDictFind: DefaultJob [Size=8192]
