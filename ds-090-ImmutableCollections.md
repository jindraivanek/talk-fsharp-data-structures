<!-- header: '**F# Data Structures**' -->

# C# Immutable collections

---

- Immutable collections are persistent data structures for C# from .NET 7
- `ImmutableList<T>` is indexable, represented as tree (similar to `Map<int, T>`)
- `ImmutableArray<T>` copying whole array on change (!)
- `ImmutableDictionary<K, V>` is similar to `Map<K, V>`
- `ImmutableStack<T>` is actually linked list - similar to `list<T>`
- `ImmutableQueue<T>` - no std. F# equivalent\

https://learn.microsoft.com/en-us/archive/msdn-magazine/2017/march/net-framework-immutable-collections

---
<style scoped>
table {
  font-size: 20px;
}
</style>

|                                     Method |       Mean |     Error |    StdDev |    Gen0 |   Gen1 | Allocated |
|------------------------------------------- |-----------:|----------:|----------:|--------:|-------:|----------:|
|                          **'int - List cons'** |   2.375 us | 0.0473 us | 0.1059 us |  2.5482 | 0.4234 |   32000 B |
|                 'int - ImmutableList cons' |  95.410 us | 1.7462 us | 1.6334 us | 40.0391 | 9.6436 |  502896 B |
|                       **'int - List.reverse'** |   2.511 us | 0.0413 us | 0.0606 us |  2.5482 | 0.4234 |   32000 B |
|              'int - ImmutableList.reverse' |  71.121 us | 0.6854 us | 0.6411 us |  3.7842 | 0.8545 |   48024 B |
|                           **'int - List.map'** |   2.781 us | 0.0543 us | 0.0687 us |  2.5482 | 0.5074 |   32000 B |
|   'int - ImmutableList map by LINQ Select' |  31.375 us | 0.5986 us | 0.7571 us |  4.1504 | 0.9766 |   52200 B |
|       'int - ImmutableList map by SetItem' | 113.180 us | 2.1415 us | 2.4661 us | 36.2549 |      - |  455376 B |
|       'int - ImmutableList map by Builder' |  36.315 us | 0.6762 us | 0.6944 us |  3.7842 | 1.0376 |   48072 B |
|                        **'int - List.filter'** |   1.756 us | 0.0350 us | 0.0623 us |  1.2741 | 0.1411 |   16000 B |
| 'int - ImmutableList filter by LINQ Where' |  13.979 us | 0.2794 us | 0.3825 us |  2.2736 | 0.2747 |   28672 B |
|  'int - ImmutableList filter by RemoveAll' |  57.953 us | 0.9039 us | 0.8455 us |  2.3804 | 0.2441 |   30376 B |
|                        **'int - List.reduce'** |   1.095 us | 0.0148 us | 0.0138 us |       - |      - |         - |
|               'int - ImmutableList.reduce' |   4.495 us | 0.0656 us | 0.0806 us |  0.0076 |      - |     112 B |
|                      **'int - List.contains'** |   5.087 us | 0.0649 us | 0.0607 us |       - |      - |      40 B |
|             'int - ImmutableList.contains' |  12.743 us | 0.1634 us | 0.1448 us |       - |      - |      72 B |

---
