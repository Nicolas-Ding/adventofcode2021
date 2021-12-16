using System;
using Utils;

internal class Day15
{
    private static int _height;
    private static int _width;
    private static bool part2 = true;

    // Main Method
    static public void Main(String[] args)
    {
        var file = Utils.Utils.ReadLinesFromFile("input.txt").ToList();

        var raw = file.Select(_ => _.Select(_ => _ - '0').ToList()).ToList();
        List<List<int>> input = null;

        // Part 2 bonus
        if (!part2)
        {
            input = raw;
        }
        else
        {
            input = new List<List<int>>();
            for (int i = 0; i < 5; i++)
            {
                for (int row = 0; row < raw.Count(); row++)
                {
                    var newList = new List<int>();
                    for (int j = 0; j < 5; j++)
                    {
                        for (int column = 0; column < raw[row].Count(); column++)
                        {
                            int val = raw[row][column] + i + j;
                            while (val > 9)
                            {
                                val = val - 9;
                            }
                            newList.Add(val);
                        }
                    }
                    input.Add(newList);
                }
            }
        }

        _height = input.Count();
        _width = input.First().Count();

        List<List<int>> minDistanceMapping = new List<List<int>>();
        for (int i = 0; i < _height; i++)
        {
            minDistanceMapping.Add(new List<int>(_width));
            for (int j = 0; j < _width; j++)
            {
                minDistanceMapping[i].Add(Int32.MaxValue);
            }
        }

        Queue<(int posY, int posX, int previousCost)> queue = new Queue<(int, int, int)> ();
        queue.Enqueue((0, 0, 0));

        while (queue.Count > 0)
        {
            var pos = queue.Dequeue();
            GoToPosition(
                (pos.posY, pos.posX),
                pos.previousCost,
                input,
                minDistanceMapping,
                queue);
        }

        Console.WriteLine(minDistanceMapping.Last().Last() - input.First().First());
    }

    static public void GoToPosition(
        (int, int) position, 
        int previousCost, 
        List<List<int>> input, 
        List<List<int>> minDistanceMapping,
        Queue<(int, int, int)> queue)
    {
        if (previousCost + input[position.Item1][position.Item2] < minDistanceMapping[position.Item1][position.Item2])
        {
            minDistanceMapping[position.Item1][position.Item2] = previousCost + input[position.Item1][position.Item2];
            foreach  (var newPos in new List<(int, int)>
                {
                    (position.Item1 + 1, position.Item2),
                    (position.Item1 - 1, position.Item2),
                    (position.Item1, position.Item2 + 1),
                    (position.Item1, position.Item2 - 1),
                })
            {
                if (newPos.Item1 >= 0 && newPos.Item2 >= 0 && newPos.Item1 < _height && newPos.Item2 < _width)
                {
                    queue.Enqueue((newPos.Item1, newPos.Item2, minDistanceMapping[position.Item1][position.Item2]));
                }
            }
        }
    }
}


