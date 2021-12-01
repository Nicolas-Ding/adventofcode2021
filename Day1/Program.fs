open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

type ENV = T | P

let env = P
let inputFile = env |> function
    | T -> "test.txt"
    | P -> "input.txt"

let lines = System.IO.File.ReadLines inputFile
                |> Seq.map int

let day1 n = 
    lines
    |> Seq.windowed n
    |> Seq.filter (fun (array) -> Array.last array > array[0])
    |> Seq.length

printfn "%d" (day1 2)
printfn "%d" (day1 4)

[<MemoryDiagnoser>]
type Day1() = 

    [<Benchmark>]
    member self.Part1 () = day1 2

    [<Benchmark>]
    member self.Part2 () = day1 4

[<EntryPoint>]
let Main args =
    BenchmarkRunner.Run typeof<Day1> |> ignore
    0