using System;
using System.Text;
using Utils;

internal class Day11
{
    // Main Method
    static public void Main(String[] args)
    {
        var file = Utils.Utils.ReadLinesFromFile("input.txt");

        string input = file.First();

        char firstLetter = input[0];
        char lastLetter = input[input.Length - 1];

        Dictionary<string, long> entries = new Dictionary<string, long>();

        for (int i = 0; i < input.Length - 1; i++)
        {
            string pair = input.Substring(i, 2);
            TryCreateEntryAndIncrement(entries, pair, 1);
        }

        Dictionary<string, char> insertions =
            file
            .Skip(2)
            .Select(_ => _.Split(" -> "))
            .ToDictionary(_ => _[0], _ => _[1][0]);

        for (int i = 0; i < 40; i++)
        {
            entries = Iterate(entries, insertions);
            Dictionary<char, long> charactersCount = new Dictionary<char, long>();
            foreach (KeyValuePair<string, long> kvp in entries)
            {
                TryCreateEntryAndIncrement(charactersCount, kvp.Key[0], kvp.Value);
                TryCreateEntryAndIncrement(charactersCount, kvp.Key[1], kvp.Value);
            }
            TryCreateEntryAndIncrement(charactersCount, firstLetter, 1);
            TryCreateEntryAndIncrement(charactersCount, lastLetter, 1);

            Console.WriteLine(charactersCount.Values.Max()/2 - charactersCount.Values.Min()/2);
        }
    }

    public static void TryCreateEntryAndIncrement<T>(Dictionary<T, long> dict, T key, long increment)
    {
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, 0);
        }
        dict[key] += increment;
    }

    static public Dictionary<string, long> Iterate(Dictionary<string, long> entries, Dictionary<string, char> insertions)
    {
        Dictionary<string , long> result = new Dictionary<string , long>();
        foreach (var kvp in entries)
        {
            if (insertions.ContainsKey(kvp.Key))
            {
                string part1 = new string( new char[] { kvp.Key[0], insertions[kvp.Key] });
                string part2 = new string(new char[] { insertions[kvp.Key], kvp.Key[1] });
                TryCreateEntryAndIncrement(result, part1, kvp.Value);
                TryCreateEntryAndIncrement(result, part2, kvp.Value);
            }
            else
            {
                TryCreateEntryAndIncrement(result, kvp.Key, kvp.Value);
            }
        }
        return result;
    }
}


