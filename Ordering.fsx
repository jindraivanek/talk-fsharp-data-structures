type R = {A: int; B: string}
{A = 1; B = "b"} < {A = 2; B = "a"}
{A = 1; B = "a"} = {A = 1; B = "a"}
{A = 1; B = "a"} < {A = 1; B = "b"}

type R2 = {B: string; A: int}
{B = "b"; A = 1} > {B = "a"; A = 2}
{B = "a"; A = 2} > {B = "a"; A = 1}

Some 1 < Some 2
None < Some System.Int32.MaxValue

System.Double.NaN <> System.Double.NaN

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