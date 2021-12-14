using Utils;

using System;

class Day11
{
    public static int Count = 0;

    // Main Method
    static public void Main(String[] args)
    {
        var file = Utils.Utils.ReadLinesFromFile("input.txt");

        var matrix = file
            .Select(x => x.ToCharArray().ToList().Select(i => i - '0').ToList())
            .ToList();

        int height = matrix.Count;
        int width = matrix[0].Count;

        for (int n = 0; n < 10000; n++)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Increment(matrix, i, j);
                }
            }
            int flashNb = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (matrix[i][j] > 9)
                    {
                        matrix[i][j] = 0;
                        flashNb++;
                    }
                }
            }
            if (flashNb == 100)
            {
                Console.WriteLine(n+1);
                return;
            }
        }
        Console.WriteLine(Count);
    }

    static public void Increment(List<List<int>> table, int i, int j)
    {
        if (table[i][j] > 9)
        {
            return;
        }
        table[i][j] += 1;
        if (table[i][j] == 10)
        {
            Count++;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (i + x >= 0 && j + y >= 0 && i + x < table.Count && j + y < table[0].Count)
                    {
                        Increment(table, i + x, j + y);
                    }
                }
            }
        }
    }
}


