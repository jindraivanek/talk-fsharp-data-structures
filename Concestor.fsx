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

let c: Conc<Dictionary<string,int>> = Conc<Dictionary<_,_>>(
    (fun () -> Dictionary<_,_>()),
    (fun k v d -> d.Add(k, v)),
    (fun k d -> d.Remove(k) |> ignore),
    (fun k v d -> d[k] <- v),
    (fun k d -> d[k]))
let c0 = c.empty
let c1 = c.add "a" 1 c0
let c2 = c1 |> c.add "b" 2 |> c.update "a" 42
let c3 = c.add "c" 3 c2
(c.get c3) |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> printfn "%A"
(c.get c2) |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> printfn "%A"
(c.get (c2 |> c.remove "b" |> c.add "bb" 3)) |> Seq.map (fun kvp -> kvp.Key, kvp.Value) |> printfn "%A"
c1
c.get c1
c.get c2