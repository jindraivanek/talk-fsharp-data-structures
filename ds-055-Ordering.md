<!-- header: '**F# Data Structures**' -->

# F# data types
* unit
* primitive types - `int`, `float`, `string`, `bool`, ...
* records
* tuples
* discriminated unions

## composed types

* `list`
* `Set`
* `Map`

all F# data types have defined structural equality and ordering - can be used in `Set` and `Map`

---

# Ordering
Ordering by field/case position, then recurse or prim. type ordering

```fsharp
type R = {A: int; B: string}
{A = 1; B = "b"} < {A = 2; B = "a"}
{A = 1; B = "a"} = {A = 1; B = "a"}
{A = 1; B = "a"} < {A = 1; B = "b"}

type R2 = {B: string; A: int}
{B = "b"; A = 1} > {B = "a"; A = 2}
{B = "a"; A = 2} > {B = "a"; A = 1}

("a", 1) < ("a", 2)

//DU - by order of cases

Some 1 < Some 2
None < Some System.Int32.MaxValue
```

---

(Ab)use of ordering example

```fsharp
type PokerHand =
    | HighCard of int
    | Pair of int
    | TwoPair of int * int
    | ThreeOfAKind of int
    | Straight of int
    | Flush of int
    | FullHouse of int * int
    | FourOfAKind of int
    | StraightFlush of int
    | RoyalFlush
```

---
