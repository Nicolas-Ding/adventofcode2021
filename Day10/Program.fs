type ENV = T | P

let env = P

let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

let lines = 
    System.IO.File.ReadLines inputFile

let getScore char = 
    match char with 
    | ')' -> 3
    | ']' -> 57
    | '}' -> 1197
    | '>' -> 25137
    | _ -> failwithf "%c" char

let getMatchingSymbol char = 
    match char with 
    | '(' -> ')'
    | '[' -> ']'
    | '{' -> '}'
    | '<' -> '>'
    | _ -> failwithf "%c" char

let isOpening char = 
    char = '(' || char = '['  || char = '{'  || char = '<' 

let rec part1Score formula expectedSigns = 
    match formula, expectedSigns with 
        | c::t, e::u when c = e -> part1Score t u
        | c::t, expectedSigns when isOpening c -> part1Score t ((getMatchingSymbol c)::expectedSigns)
        | c::t, _ -> getScore c
        | [], _ -> 0

let res1 = 
    lines
        |> Seq.map (fun formula -> part1Score (formula |> Seq.toList) [])
        |> Seq.sum

printfn "%d" res1

let getCharacterScorePart2 c = 
    match c with 
        | ')' -> 1L
        | ']' -> 2L
        | '}' -> 3L
        | '>' -> 4L
        | _ -> failwithf "%c" c

let rec getCompletionScore completionSymbols tempRes = 
    match completionSymbols with 
        | h::t -> getCompletionScore t (tempRes * 5L + getCharacterScorePart2 h)
        | [] -> tempRes

let rec part2 formula expectedSigns = 
    match formula, expectedSigns with 
        | c::t, e::u when c = e -> part2 t u
        | c::t, expectedSigns when isOpening c -> part2 t ((getMatchingSymbol c)::expectedSigns)
        | c::t, _ -> failwith "should never happen"
        | [], expectedSigns -> expectedSigns

let res2 = 
    lines
        |> Seq.filter (fun formula -> part1Score (formula |> Seq.toList) [] = 0)
        |> Seq.map (fun formula -> part2 (formula |> Seq.toList) [])
        |> Seq.map (fun i -> getCompletionScore i 0L)
        |> Seq.sort

printfn "%d" (res2 |> Seq.skip((res2 |> Seq.length) / 2) |> Seq.head)