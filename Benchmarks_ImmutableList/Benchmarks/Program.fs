module ListBenchmarks

open System
open System.Linq
open System.Collections.Immutable
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

type MyRecord = { Id: int; Name: string }

type DU =
    | A of int
    | B of string

let inline test xs f =
    for i in xs do
        f i xs |> ignore

let size = 1000
let listOfInts size = [ 1..size ]
let input<'T>() : 'T list =
    let listOfStrings = listOfInts size |> List.map (fun i -> i.ToString())
    let listOfRecords =
        listOfInts size |> List.map (fun i -> { Id = i; Name = i.ToString() })

    match typeof<'T> with
    | t when t = typeof<int> -> listOfInts size |> Seq.cast<'T> |> List.ofSeq
    | t when t = typeof<string> -> listOfStrings |> Seq.cast<'T> |> List.ofSeq
    | t when t = typeof<MyRecord> -> listOfRecords |> Seq.cast<'T> |> List.ofSeq

[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<string>)>]
[<GenericTypeArguments(typeof<MyRecord>)>]
type ListFunctions<'T>() =
    let xs = input<'T>()

    // [<Benchmark>]
    // member _.ListRev() =
    //     List.rev xs

    [<Benchmark>]
    member _.ListAddToEnd() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (acc @ [i])
        go size []

    [<Benchmark>]
    member _.ListAddToEndAcc() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (i :: acc)
        go size [] |> List.rev

    [<Benchmark>]
    member _.ListRevIndexed() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (xs.[i] :: acc)
        go (size-1) []

type ListVsSet() =
    [<Params(64, 128, 256, 512, 1024, 8192)>]
    member val Size = 0 with get, set

    member x.ListOfInts = listOfInts x.Size
    member x.SetOfInts = x.ListOfInts |> Set.ofList
    
    [<Benchmark>]
    member x.ListContains() =
        test x.ListOfInts List.contains

    [<Benchmark>]
    member x.SetContains() =
        test x.SetOfInts Set.contains

type Set() =
    let setOfInts = listOfInts size |> set
    
    [<Benchmark>]
    member _.SetItemUnionByAdd() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (Set.add (size+i) acc)
        go size setOfInts

    [<Benchmark>]
    member _.SetUnion() =
        Set.union setOfInts (listOfInts size |> List.map (fun i -> size + i) |> set)
        
[<MemoryDiagnoser>]
type ListImmListTests() =
    let size = 1000
    let listOfInts = [ 1..size ]
    let listOfIntsSmaller = [ 1..100 ]
    let listOfStrings = listOfInts |> List.map (fun i -> i.ToString())
    let listOfRecords =
        listOfInts |> List.map (fun i -> { Id = i; Name = i.ToString() })

    let immListOfInts = listOfInts |> ImmutableList.CreateRange
    let immListOfIntsSmaller = listOfIntsSmaller |> ImmutableList.CreateRange

    [<Benchmark(Description = "int - List cons")>]
    member _.IntListCons() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (i :: acc)
        go size []

    [<Benchmark(Description = "int - ImmutableList cons")>]
    member _.IntImmListCons() =
        let rec go i (acc: ImmutableList<_>) =
            if i = 0 then acc
            else go (i - 1) (acc.Add i)
        go size ImmutableList<int>.Empty

    [<Benchmark(Description = "int - List.reverse")>]
    member _.IntListReverse() =
        listOfInts |> List.rev

    [<Benchmark(Description = "int - ImmutableList.reverse")>]
    member _.IntImmListReverse() =
        immListOfInts.Reverse()

    [<Benchmark(Description = "int - List.map")>]
    member _.IntListMap() =
        listOfInts |> List.map ((+) 1)

    [<Benchmark(Description = "int - ImmutableList map by LINQ Select")>]
    member _.IntImmListMap() =
        immListOfInts.Select ((+) 1) |> ImmutableList.CreateRange

    [<Benchmark(Description = "int - ImmutableList map by SetItem")>]
    member _.IntImmListMapSetItem() =
        let mutable xs = immListOfInts
        for i=0 to size-1 do
            xs <- immListOfInts.SetItem(i, i + 1)
        xs

    [<Benchmark(Description = "int - ImmutableList map by Builder")>]
    member _.IntImmListMapBuilder() =
        //let mutable xs = immListOfInts
        let b = immListOfInts.ToBuilder()
        for i=0 to size-1 do
            b[i] <- i + 1
        b.ToImmutable()

    [<Benchmark(Description = "int - List.filter")>]
    member _.IntListFilter() =
        listOfInts |> List.filter (fun i -> i % 2 = 0)

    [<Benchmark(Description = "int - ImmutableList filter by LINQ Where")>]
    member _.IntImmListFilter() =
        immListOfInts.Where (fun i -> i % 2 = 0) |> ImmutableList.CreateRange

    [<Benchmark(Description = "int - ImmutableList filter by RemoveAll")>]
    member _.IntImmListFilterRemoveAll() =
        immListOfInts.RemoveAll (fun i -> i % 2 <> 0)

    [<Benchmark(Description = "int - List.reduce")>]
    member _.IntListReduce() =
        listOfInts |> (List.reduce (+))

    [<Benchmark(Description = "int - ImmutableList.reduce")>]
    member _.IntImmListReduce() =
        let mutable x = 0
        immListOfInts.ForEach (fun y -> x <- x + y)

    [<Benchmark(Description = "int - List.contains")>]
    member _.IntListContains() =
        test listOfIntsSmaller List.contains

    [<Benchmark(Description = "int - ImmutableList.contains")>]
    member _.IntImmListContains() =
        test immListOfIntsSmaller (fun i xs -> xs.Contains i)

open System.Collections.Generic

type Operation =
    | Add
    | Remove
    | Update

type Delta<'k,'v,'t> =
    | Delta of Operation * 'k * 'v * 'v * Concestor<'k,'v,'t>
    | Data of 't

and Concestor<'k,'v,'t> = {
    mutable Ancestry : Delta<'k, 'v, 't>
    LockObj: obj
}

let create d = { Ancestry = Data d; LockObj = obj() }

type Conc<'t>(emptyF, addF, removeF, updateF, getF) =
    let apply c d op k v nv =
        match op with
        | Add ->
            addF k v d
            Delta (Remove, k, v, Unchecked.defaultof<_>, c)
        | Remove ->
            removeF k d
            Delta (Add, k, v, Unchecked.defaultof<_>, c)
        | Update ->
            updateF k nv d
            Delta (Update, k, nv, v, c)

    let rec ossifyCPS concestor k =
      match concestor.Ancestry with
      | Data s -> k s
      | Delta(op, key, value, newValue, relative) ->
          ossifyCPS relative (fun (d: 't) ->
            relative.Ancestry <-
              apply concestor d op key value newValue
            // OPTIM: We only need to set this once.
            concestor.Ancestry <- Data d
            k d)

    let ossify concestor = ossifyCPS concestor id

    let locked concestor f =
        lock concestor.LockObj (fun () -> f(ossify concestor))

    member __.empty = create <| emptyF()

    member __.add key value concestor =
        locked concestor (fun d ->
            let curr = { concestor with Ancestry = Data d }
            let ancestry =
              addF key value d
              Delta(Remove, key, value, Unchecked.defaultof<_>, curr)
            concestor.Ancestry <- ancestry
            curr)

    member __.remove key concestor =
        locked concestor (fun d ->
            let curr = { concestor with Ancestry = Data d }
            let ancestry =
              let value = getF key d
              removeF key d
              Delta(Add, key, value, Unchecked.defaultof<_>, curr)
            concestor.Ancestry <- ancestry
            curr)

    member __.update key newValue concestor =
        locked concestor (fun d ->
            let curr = { concestor with Ancestry = Data d }
            let ancestry =
              let value = getF key d
              updateF key newValue d
              Delta(Update, key, newValue, value, curr)
            concestor.Ancestry <- ancestry
            curr)

    member _.get concestor =
        locked concestor id

let mapC: Conc<Dictionary<string,int>> = Conc<Dictionary<_,_>>(
    (fun () -> Dictionary<_,_>()),
    (fun k v d -> d.Add(k, v)),
    (fun k d -> d.Remove(k) |> ignore),
    (fun k v d -> d[k] <- v),
    (fun k d -> d[k]))

type MapVsConcestorDict() =
    [<Params(64, 128, 256, 512, 1024, 8192)>]
    member val Size = 0 with get, set

    member x.MapOfInts = lazy (listOfInts x.Size |> List.map (fun x -> x.ToString(), x) |> Map.ofList)
    member x.ConcestorDictOfInts = lazy (listOfInts x.Size |> List.map (fun i -> i.ToString(), i) |> Seq.fold (fun d (k, v) -> mapC.add k v d) mapC.empty)

    [<Benchmark>]
    member x.MapFind() =
        for k in Map.keys x.MapOfInts.Value do
            x.MapOfInts.Value.[k] |> ignore

    [<Benchmark>]
    member x.ConcestorDictFind() =
        for k in Map.keys x.MapOfInts.Value do
            (mapC.get x.ConcestorDictOfInts.Value).[k] |> ignore

    // [<Benchmark>]
    // member x.MapAdd() =
    //     test x.MapOfInts (fun i xs -> Map.add (size+i) (size+i) xs)

    // [<Benchmark>]
    // member x.ConcestorDictAdd() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.add (size+i) (size+i) xs)

    // [<Benchmark>]
    // member x.MapRemove() =
    //     test x.MapOfInts (fun i xs -> Map.remove i xs)

    // [<Benchmark>]
    // member x.ConcestorDictRemove() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.remove i xs)

    // [<Benchmark>]
    // member x.MapUpdate() =
    //     test x.MapOfInts (fun i xs -> Map.add i (size+i) xs)

    // [<Benchmark>]
    // member x.ConcestorDictUpdate() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.update i (size+i) xs)

    // [<Benchmark>]
    // member x.MapTryFind() =
    //     test x.MapOfInts (fun i xs -> Map.tryFind i xs)

    // [<Benchmark>]
    // member x.ConcestorDictTryFind() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.get xs |> Map.tryFind i)

    // [<Benchmark>]
    // member x.MapTryFindOpt() =
    //     test x.MapOfInts (fun i xs -> Map.tryFindOpt i xs)

    // [<Benchmark>]
    // member x.ConcestorDictTryFindOpt() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.get xs |> Map.tryFindOpt i)

//InProcessEmitToolchain.Instance
//BenchmarkRunner.Run<ListImmListTests>()
// BenchmarkRunner.Run<ListVsSet>()
BenchmarkRunner.Run<MapVsConcestorDict>()
// BenchmarkRunner.Run<GenericTests<int>>()
// BenchmarkRunner.Run<GenericTests<string>>()
// BenchmarkRunner.Run<GenericTests<MyRecord>>()
//BenchmarkSwitcher.FromTypes([| typeof<ListFunctions<int>>; typeof<ListFunctions<string>> |]).RunAllJoined()
//BenchmarkSwitcher.FromAssembly(typeof<MyRecord>.Assembly).RunAllJoined()

//BenchmarkRunner.Run([| typeof<ListTests>; typeof<ArrayTests>; typeof<SeqTests> |])
