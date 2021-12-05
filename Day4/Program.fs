open System
open System.Collections.Generic

type ENV = T | P

let env = T
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

let lines = 
    System.IO.File.ReadLines inputFile

let numbers = 
    lines
        |> Seq.take(1)

let bingoBoards = 
    lines
        |> Seq.skip(1)
        |> Seq.chunkBySize 6
        |> Seq.map (Array.skip 1)
        |> Seq.map (Array.map (fun i -> i.Split(' ')))
        |> Seq.map (Array.map (Array.map (fun i -> i, false)))

let isHorizontalBingo bingoB = bingoB |> Array.exists (fun row -> row |> Array.forall (fun (_, p) -> p = true))

let isBingo bingoBoard = 
    bingoBoard 
    |> isHorizontalBingo
    &&
    bingoBoard
    |> Array.transpose
    |> isHorizontalBingo

let populateDict bingoBoard (dict:Dictionary<int, int*int>) = 
    bingoBoard 
    |> Array.iteri (fun i line -> 
        line
        |> Array.iteri (fun j (v,b) -> dict.Add(v, (i,j))))
    
//let bingoScore bingoBoard numbersList = 
//    let dict = Dictionary<int, int*int>();
//    populateDict bingoBoard dict;
//    numbersList
//    |> Seq.iter (fun n -> 
//        let (x,y) = dict[n];
//        let (a,b) = bingoBoard[x][y];
//        bingoBoard[x][y] <- a, true;
//        if isBingo bingoBoard then return 1)





