open System

type ENV = T | P

let env = P
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

let lines = 
    System.IO.File.ReadLines inputFile
        |> Seq.map Seq.toList
        |> Seq.map (List.map (fun(c) -> if c = '1' then 1 else 0))

let rec accumulate arr binary = 
    match (arr, binary) with
        | (a::t, 0::tail) -> (a-1)::(accumulate t tail)
        | (a::t, 1::tail) -> (a+1)::(accumulate t tail)
        | ([], 0::tail) -> -1::(accumulate [] tail)
        | ([], 1::tail) -> 1::(accumulate [] tail)
        | (_, []) -> arr
        | _ -> failwith "Invalid string !"

let stringLength = (lines |> Seq.head).Length

let countOccurrences lines = 
    lines
    |> Seq.fold accumulate []

let rec toIntRec value b = 
    match b with 
        | a::t when a >= 0 -> toIntRec (2*value) t
        | a::t when a < 0 -> toIntRec (2*value + 1) t
        | [] -> value
        | _ -> failwith "Invalid binary array !"

let toInt = toIntRec 0        

let rec reverse b = 
    match b with
        | a::t -> -a::(reverse t)
        | [] -> []
    
let totalCount = countOccurrences lines;

printfn "Part 1 : %d" ((toInt totalCount) * (toInt (reverse totalCount)))

let step (lines:seq<int list>) a b n = 
    let countOcc = (if (countOccurrences lines)[n] >= 0 then a else b);
    lines
    |> Seq.filter (fun(array) -> array[n] = countOcc)
    
let rec part2 (lines:seq<int list>) a b n = 
    match (lines |> Seq.length) with
        | 1 -> lines
        | _ -> part2 (step lines a b n) a b (n+1)

let convertToDecimal binaryString = System.Convert.ToInt32(binaryString, 2)        

let binaryToInt (b:int list) = 
    b 
    |> List.map string 
    |> fun (strs) -> String.concat "" strs
    |> convertToDecimal

printfn "Part 2 : %d" ((binaryToInt (part2 lines 0 1 0 |> Seq.head))*(binaryToInt (part2 lines 1 0 0 |> Seq.head)))
