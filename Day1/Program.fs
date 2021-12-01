open System
type ENV = T | P

let env = P
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

let lines = System.IO.File.ReadLines inputFile
                |> Seq.map int

//let result1a = 
//    lines
//    |> Seq.pairwise
//    |> Seq.filter (fun (a, b) -> b > a)
//    |> Seq.length

//printfn "part1 : %d" result1a

//let rec pb1a list = 
//    match list with 
//    | start::second::tail -> Convert.ToInt32(second > start) + pb1a (second::tail)
//    | [_] -> 0

//printfn "part1 : %d" (pb1a (List.ofSeq lines))

//let result2a = 
//    lines
//    |> Seq.windowed 3
//    |> Seq.pairwise
//    |> Seq.filter (fun (a, b) -> (b |> Array.sum) > (a |> Array.sum))
//    |> Seq.length

//printfn "part 2 : %d" result2a

//let result2b = 
//    lines
//    |> Seq.windowed 4
//    |> Seq.filter (fun (array) -> Array.last array > array[0])
//    |> Seq.length

//printfn "part 2 : %d" result2b

let day1 n = 
    lines
    |> Seq.windowed n
    |> Seq.filter (fun (array) -> Array.last array > array[0])
    |> Seq.length

printfn "%d" (day1 2)
printfn "%d" (day1 4)