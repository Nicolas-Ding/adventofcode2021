open System
open System.Collections.Generic

type ENV = T | P

let env = P
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

let lines = 
    System.IO.File.ReadLines inputFile

let memoize f =
   let cache = Dictionary<_,_>()
   let rec frec a b = 
       let value = ref 0L
       let exist = cache.TryGetValue((a,b), value)
       match exist with
       | true -> ()
       | false ->
           value := f frec a b
           cache.Add((a,b), !value)
       !value
   in frec 

let f = 
    fun calculateFishNumber (fishAge:int64) (daysLeft:int64) ->
    match daysLeft with 
    | 0L -> 1L
    | _ -> match fishAge with 
           | 0L -> calculateFishNumber 6L (daysLeft - 1L) + calculateFishNumber 8L (daysLeft - 1L)
           | _ -> calculateFishNumber (fishAge - 1L) (daysLeft - 1L)

let memoCalculate = memoize f

let result =
    lines
    |> Seq.head
    |> fun i -> i.Split(',')
    |> Array.map Int32.Parse
    |> Array.map (fun i -> memoCalculate i 256)
    |> Seq.sum

printfn "%d" result
