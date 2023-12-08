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

---

```fsharp
[<NoEquality; NoComparison>]
[<AllowNullLiteral>]
type internal MapTree<'Key, 'Value>(k: 'Key, v: 'Value, h: int) =
    member _.Height = h
    member _.Key = k
    member _.Value = v
    new(k: 'Key, v: 'Value) = MapTree(k, v, 1)

[<NoEquality; NoComparison>]
[<Sealed>]
[<AllowNullLiteral>]
type internal MapTreeNode<'Key, 'Value>
    (
        k: 'Key,
        v: 'Value,
        left: MapTree<'Key, 'Value>,
        right: MapTree<'Key, 'Value>,
        h: int
    ) =
    inherit MapTree<'Key, 'Value>(k, v, h)
    member _.Left = left
    member _.Right = right
```

---

Insert = search + add

```fsharp
let m2 = m |> Map.add 35
```

![tree insert](tree-insert.gif)

from https://visualgo.net/en/bst

---

- keys must be comparable
- searching for item (`Map.find`, `Map.containsKey`) by binary search
- insert, remove - unchanged part of tree is shared
![after insert](map_after_insert.png)
- functions with predicate on key (`Map.pick`, `Map.findKey`), goes through whole tree! (in keys order)
example
- keys cannot be duplicite - insert (`Map.add`) repace value if key already exists

---

Creation of `Map` - List.groupBy

```fsharp
[1..1000] |> List.groupBy (fun x -> x % 100) |> Map.ofList
```

---
