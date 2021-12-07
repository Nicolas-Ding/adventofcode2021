open System
open System.Collections.Generic

type ENV = T | P

let env = P
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

let lines = 
    System.IO.File.ReadLines inputFile
    |> Seq.head
    |> fun i -> i.Split(',')
    |> Array.map Int32.Parse

let inline median input = 
    let sorted = input |> Seq.toArray |> Array.sort
    sorted[(sorted.Length / 2) - 1]

let bestPosition = median lines

let part1 = 
    lines 
    |> Seq.map (fun i -> abs (bestPosition - i))
    |> Seq.sum

printfn "%d" part1

let newConsumption lines position = 
    lines 
    |> Seq.map (fun i -> (abs (position - i)) * (abs (position - i) + 1) / 2)
    |> Seq.sum

let part2 = 
    seq {lines |> Seq.min .. lines |> Seq.max}
    |> Seq.map (fun i -> newConsumption lines i)
    |> Seq.min

printfn "%d" part2
