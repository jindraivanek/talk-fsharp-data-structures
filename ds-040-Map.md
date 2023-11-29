<!-- header: '**F# Data Structures**' -->

# Structural sharing

---

# F# Map

Dictionary like immutable data structure

```fsharp
let mapA = Map.ofList [1, "A"; 2, "B"; 3, "C"]
let mapB = Map.ofList [1, "A"; 2, "B"; 3, "C"; 4, "D"]
let mapB2 = Map.add 4 "D" mapA
mapB = mapB2 // true
```

---

Internally implemented as a (balanced) tree

```fsharp
let m = [11; 20; 29; 32; 41; 50; 65; 72; 91; 99] |> List.map (fun x -> x, string x) |> Map.ofList
```

![Example tree](map1.png)

DEMO: https://visualgo.net/en/bst

TODO: make video?

---

- keys must be comparable
- searching for item (`Map.find`, `Map.containsKey`) by binary search
- insert, remove - unchanged part of tree is shared
- functions with predicate on key (`Map.pick`, `Map.findKey`), goes through whole tree! (in keys order)
- keys cannot be duplicite - insert (`Map.add`) repace value if key already exists

---

```fsharp
[1..1000] |> List.groupBy (fun x -> x % 100) |> Map.ofList
```

---
