using Utils;

using System;

class Node
{
    public string Name;

    public List<Node> Nodes = new List<Node>();

    public bool IsSmallCave { get; set; }
}

class Day11
{
    public static int Count = 0;

    // Main Method
    static public void Main(String[] args)
    {
        var file = Utils.Utils.ReadLinesFromFile("input.txt");
        Dictionary<string, Node> nodes = new Dictionary<string, Node>();
        foreach (string line in file)
        {
            var lineArray = line.Split('-');
            for (int i = 0; i < 2; i++)
            {
                if (!nodes.ContainsKey(lineArray[i]))
                {
                    nodes[lineArray[i]] = new Node()
                    {
                        IsSmallCave = lineArray[i].ToLower() == lineArray[i],
                        Name = lineArray[i]
                    };
                }
            }
            nodes[lineArray[0]].Nodes.Add(nodes[lineArray[1]]);
            nodes[lineArray[1]].Nodes.Add(nodes[lineArray[0]]);
        }
        Console.WriteLine(ComputeResultsFromPoint("start", nodes, new List<string>() { "start" }));
    }

    static public int ComputeResultsFromPoint(string startPoint, Dictionary<string, Node> nodes, List<string> visitedSmall, bool visitedOneDuplicate = false)
    {
        if (startPoint == "end")
        {
            Console.WriteLine(string.Join(",", visitedSmall));
            return 1;
        }
        var startNode = nodes[startPoint];
        int res = 0;
        foreach (Node n in startNode.Nodes)
        {
            if (n.Name != "start" && (!n.IsSmallCave || visitedSmall.Count(_ => _ == n.Name) == 0))
            {
                visitedSmall.Add(n.Name);
                res += ComputeResultsFromPoint(n.Name, nodes, visitedSmall, visitedOneDuplicate);
                visitedSmall.Remove(n.Name);
            }
            else if (!visitedOneDuplicate && n.Name != "start")
            {
                visitedSmall.Add(n.Name);
                res += ComputeResultsFromPoint(n.Name, nodes, visitedSmall, true);
                visitedSmall.Remove(n.Name);
            }
        }
        return res;
    }
    
}


