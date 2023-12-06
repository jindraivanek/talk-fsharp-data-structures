<!-- header: '**F# Data Structures**' -->

# Pure functions

---

- **Pure** function:
    - always returns the same result for the same input (**referential transparency**)
    - no side effects

- Immutable data structures allows us to write **pure** functions.

- no mutable variables / data structures, no side effects => **referential transparency**

---

- BUT:
- **referential transparency** can be achived even with mutable data structures
- mutable variables and data structures are perfectly fine when not leaking outside of function

---

```fsharp
    [<CompiledName("Fold")>]
    let fold<'T, 'State> folder (state: 'State) (list: 'T list) =
        match list with
        | [] -> state
        | _ ->
            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt (folder)
            let mutable acc = state

            for x in list do
                acc <- f.Invoke(acc, x)

            acc
```


---

### Memoize function:

```fsharp
let memoizeBy projection f =
    let cache = System.Collections.Concurrent.ConcurrentDictionary()
    fun x -> cache.GetOrAdd(projection x, lazy f x).Value
```

---
