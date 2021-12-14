using Utils;

using System;

class Day11
{
    public static int Count = 0;

    // Main Method
    static public void Main(String[] args)
    {
        var file = Utils.Utils.ReadLinesFromFile("input.txt");

        var points = file
            .TakeWhile(_ => _.Length > 0)
            .Select(_ => _.Split(',').Select(Int32.Parse).ToArray())
            .Select(_ => (_[1], _[0]))
            .ToHashSet();

        var instructions = file
            .SkipWhile(_ => _.Length > 0)
            .Skip(1)
            .Select(_ => _.Substring(11))
            .Select(_ => _.Split('='));

        foreach (string[] instruction in instructions)
        {
            string direction = instruction[0];
            int position = int.Parse(instruction[1]);
            HashSet<(int, int)> result = new HashSet<(int, int)>();
            foreach ((int, int) value in points)
            {
                if (instruction[0] == "x" && value.Item2 > position)
                {
                    result.Add((value.Item1, 2 * position - value.Item2));
                }
                else if (instruction[0] == "y" && value.Item1 > position)
                {
                    result.Add((2 * position - value.Item1, value.Item2));
                }
                else if ((instruction[0] == "x" && value.Item2 != position) ||
                        (instruction[0] == "y" && value.Item1 != position))
                {
                    result.Add(value);
                }
            }
            points = result;
            Console.WriteLine(points.Count);
        }

        Console.Clear();
        foreach (var point in points)
        {
            Console.SetCursorPosition(point.Item2, point.Item1);
            Console.Write('X');
        }
        Console.SetCursorPosition(20, 20);
        Console.ReadLine();
    }
}


