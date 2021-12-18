using Newtonsoft.Json.Linq;
using System;
using Utils;

internal class Day18
{
    // Main Method
    static public void Main(String[] args)
    {
        var file = Utils.Utils.ReadLinesFromFile("input.txt");
        Node current = null;

        // Part 1
        foreach (string line in file)
        {
            Node node = new Node(JToken.Parse(line));
            if (current == null)
            {
                current = node;
            }
            else
            {
                Node newNode = new Node(
                    left: current,
                    right: node);
                current = newNode;
            }

            Console.WriteLine("=======");
            // Console.WriteLine($"After addition {current}");
            Reduce(current);

            Console.WriteLine(current);
            Console.WriteLine(current.Magnitude);
        }

        // Part 2
        int maxMag = 0;
        foreach (string line1 in file)
        {
            foreach (string line2 in file)
            {
                var newNode = new Node(
                    left: new Node(JToken.Parse(line1)),
                    right: new Node(JToken.Parse(line2)));
                Reduce(newNode);
                var newMag = newNode.Magnitude;

                if (newMag > maxMag)
                {
                    maxMag = newMag;
                }
            }
        }
        Console.WriteLine(maxMag);
    }

    public static void Reduce(Node current)
    {
        bool hasChanged = false;

        do
        {
            hasChanged = false;
            // EXPLODE
            Node nodeToExplode = current.FindExplosionNode(4);
            if (nodeToExplode != null)
            {
                hasChanged = true;
                nodeToExplode.Explode();
                //Console.WriteLine($"After explosion:  {current}");
                continue;
            }

            // SPLIT (if not explode)
            Node nodeToSplit = current.FindSplitNode();
            if (nodeToSplit != null)
            {
                hasChanged = true;
                nodeToSplit.Split();
                //Console.WriteLine($"After split:  {current}");
                continue;
            }

        } while (hasChanged);
    }
}


