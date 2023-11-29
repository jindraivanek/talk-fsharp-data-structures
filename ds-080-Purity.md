<!-- header: '**F# Data Structures**' -->

# Pure functions

---

Immutable data structures allows us to write pure functions.

Pure function:
- always returns the same result for the same input (referential transparency)
- no side effects

---

- BUT:
- referential transparency can be achived even with mutable data structures
- mutable variables and data structures are perfectly fine when not leaking outside of function

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

- no mutable variables / data structures => referential transparency

---

Memoize function:

```fsharp
let memoizeBy projection f =
    let cache = System.Collections.Concurrent.ConcurrentDictionary()
    fun x -> cache.GetOrAdd(projection x, lazy f x).Value
```

---
