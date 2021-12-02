// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System

type ENV = T | P

let env = P
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

let lines = System.IO.File.ReadLines inputFile
                |> Seq.map (fun a -> a.Split(' '))
                |> Seq.map (fun l -> (l[0], int(l[1])))
                |> List.ofSeq

let rec move depth pos list = 
    match list with 
        | ("forward", n)::tail -> move depth (pos + n) tail
        | ("down", n)::tail -> move (depth + n) pos tail
        | ("up", n)::tail -> move (depth - n) pos tail
        | _ -> depth * pos

let part1 = move 0 0 lines

let rec moveWithAim depth pos aim list = 
    match list with 
        | ("forward", n)::tail -> moveWithAim (depth + aim * n) (pos + n) aim tail
        | ("down", n)::tail -> moveWithAim depth pos (aim + n) tail
        | ("up", n)::tail -> moveWithAim depth pos (aim - n) tail
        | _ -> depth * pos

let part2 = moveWithAim 0 0 0 lines

[<EntryPoint>]
let main argv =
    printfn "%d" part1
    printfn "%d" part2
    0 // return an integer exit code