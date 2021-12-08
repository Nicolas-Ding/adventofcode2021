open System
open System.Collections.Generic

type ENV = T | P

let env = P
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

(* Part 1 *)

let res1 = 
    System.IO.File.ReadLines inputFile
    |> Seq.map (fun i -> i.Split('|')[1])
    |> Seq.map (fun i -> i.Trim())
    |> Seq.map (fun i -> i.Split(' '))
    |> Seq.map (fun arr -> arr |> Seq.filter (fun s -> s.Length = 2 || s.Length = 3 || s.Length = 4 || s.Length = 7))
    |> Seq.map Seq.length
    |> Seq.sum

printfn "Part 1 : %d" res1

(* Part 2 *)

let filterByLength n = (fun (i:string) -> i.Length = n)

let deduce inputArray (unknownArray:string list) =
    let cf = inputArray |> Array.filter (filterByLength 2) |> Array.head |> Seq.toList
    let bdcf = inputArray |> Array.filter (filterByLength 4) |> Array.head |> Seq.toList
    let countLetters numberString letterToCountArray = 
        numberString |> Seq.filter (fun c -> List.contains c letterToCountArray) |> Seq.length
    let computeDigit (digitString:string) = 
        match digitString.Length with 
            | 2 -> 1
            | 3 -> 7
            | 4 -> 4
            | 5 when (countLetters digitString cf) = 2 -> 3
            | 5 when (countLetters digitString bdcf) = 3 -> 5
            | 5 -> 2
            | 6 when (countLetters digitString bdcf) = 4 -> 9
            | 6 when (countLetters digitString cf) = 2 -> 0
            | 6 -> 6
            | 7 -> 8
    let rec computeNumber numberString res =
        match numberString with 
            | h::t -> computeNumber t (res * 10 + (computeDigit h))
            | [] -> res
    computeNumber unknownArray 0

let res2 = 
    System.IO.File.ReadLines inputFile
    |> Seq.map (fun i -> (i.Split('|') |> Array.map (fun s -> s.Trim())))
    |> Seq.map (fun arr -> deduce (arr[0].Split(' ')) (arr[1].Split(' ') |> Array.toList))
    |> Seq.sum

printfn "%A" res2