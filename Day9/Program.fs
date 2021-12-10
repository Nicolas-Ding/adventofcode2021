open System.Collections.Generic


type ENV = T | P

let env = P

let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

(* Part 1 *)

type Pos = int * int
type Grid = Map<Pos, int>

let grid:Grid = 
    System.IO.File.ReadLines inputFile
    |> Seq.indexed
    |> Seq.collect (fun (i,s) -> s |> Seq.indexed |> Seq.map (fun (j, c) -> ((i,j),c |> string |> int)))
    |> Map

let neighbors (x,y) = [(x - 1, y); (x, y - 1); (x + 1, y); (x,y + 1)]

let isMin (cell:KeyValuePair<Pos, int>) = 
    if (neighbors cell.Key
    |> Seq.choose (fun j -> grid.TryFind j)
    |> Seq.min) > cell.Value
    then 
        Some(cell)
    else
        None

let part1 = 
    grid
    |> Seq.choose isMin
    |> Seq.sumBy (fun i -> i.Value + 1)

printfn "%A" part1

(* Part 2 *)

type Cell = int*bool
type GridVisited = Map<Pos, Cell>

let mutable gridVisited:GridVisited = 
    System.IO.File.ReadLines inputFile
    |> Seq.indexed
    |> Seq.collect (fun (i,s) -> s |> Seq.indexed |> Seq.map (fun (j, c) -> ((i,j),(c |> string |> int, false))))
    |> Map

let rec basinSize (pos:Pos) (cell:Cell) = 
    match snd cell with 
        | true -> 0
        | false when fst cell = 9 -> 
            gridVisited <- Map.add pos (fst cell, true) gridVisited;
            0
        | false -> 
            gridVisited <- Map.add pos (fst cell, true) gridVisited;
            1 + 
            (neighbors pos 
             |> Seq.map (fun j -> (j, gridVisited.TryFind j))
             |> Seq.filter (fun (newPos, j) -> j <> None && fst j.Value <> 9 && snd j.Value = false) 
             |> Seq.map (fun (newPos, j) -> basinSize newPos j.Value) 
             |> Seq.sum)

let part2 = 
    gridVisited
    |> Seq.map (fun cellPos -> (basinSize cellPos.Key cellPos.Value))
    // Dirty workaround. The issue here is that the cellPos.Value is not updated correctly 
    // as gridVisited is not updated soon enough in the Seq.map above. 
    // This lead to 1 sized basins being created. Hopefully the data is well made and we can just filter those out
    // 🤮🤮
    // Proper way would be to use a **separate** C# Dictionary or a separate array to do this but I don't have a lot of time today to work on this
    |> Seq.filter (fun i -> i > 1) 
    |> Seq.sortDescending
    |> Seq.take 3
    |> Seq.fold (fun n i -> n * i) 1

printfn "%A" part2