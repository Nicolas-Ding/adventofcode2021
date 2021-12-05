open System
open System.Collections.Generic

type ENV = T | P

let env = P
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

type Line = {
    x1:int;
    y1:int;
    x2:int;
    y2:int;
}

let lines = 
    System.IO.File.ReadLines inputFile

let map = Array2D.zeroCreate<int> 999 999; // TODO dynamic value here? 
    
let stringToLine (str:string) = 
    let a = str.Split(" -> ") |> Array.map (fun i -> i.Split(',') |> Array.map Int32.Parse)
    if a[0][0] < a[1][0] 
    then { x1 = a[0][0]; y1 = a[0][1]; x2 = a[1][0]; y2 = a[1][1]}
    else { x1 = a[1][0]; y1 = a[1][1]; x2 = a[0][0]; y2 = a[0][1]}

let (|Vertical|Other|) line = (if line.x1 = line.x2 then Vertical else Other)

let fillMap (map:int[,]) (line:Line) = 
    match line with 
        |Vertical -> 
            for y = min line.y1 line.y2 to max line.y1 line.y2 do
                map[y, line.x1] <- map[y, line.x1] + 1
        |Other -> 
            let s = sign (line.y2 - line.y1)
            for x = line.x1 to line.x2 do
                map[line.y1 + s * (x - line.x1),x] <- map[line.y1 + s * (x - line.x1),x] + 1

let inputLines = 
    lines
        |> Seq.map (fun str -> stringToLine str)
        |> Seq.iter (fillMap map)

printfn "%d" (map |> Seq.cast |> Seq.filter (fun x -> x > 1) |> Seq.length)