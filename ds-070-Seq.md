<!-- header: '**F# Data Structures**' -->

# Enumerable, seq - lazy sequences

---

Every collection implements `seq<'T>` (alias for `IEnumerable<T>`) interface.

Interface for reading elements one by one.

Lazy abstraction - elements are computed on demand.

---

```fsharp
xs |> Seq.map (fun x -> expensiveFun x) |> Seq.take 10 |> Seq.toList
```

Only first 10 elements are computed.

```fsharp
xs |> Seq.filter (...) |> Seq.map (fun x -> expensiveFun x) |> Seq.tryFind (...)
```

Only elements that pass the filter are computed.
