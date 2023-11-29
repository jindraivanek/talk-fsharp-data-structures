<!-- header: '**F# Data Structures**' -->

# C# Immutable collections

---

- Immutable collections are persistent data structures for C# from .NET 7
- `ImmutableList<T>` is indexable, represented as tree (similar to `Map<int, T>`)
- `ImmutableArray<T>` copying whole array on change (!)
- `ImmutableDictionary<K, V>` is similar to `Map<K, V>`
- `ImmutableStack<T>` is actually linked list - similar to `list<T>`
- `ImmutableQueue<T>` - no std. F# equivalent

---

https://learn.microsoft.com/en-us/archive/msdn-magazine/2017/march/net-framework-immutable-collections
