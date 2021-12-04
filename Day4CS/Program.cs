// See https://aka.ms/new-console-template for more information
using Utils;

var file = Utils.Utils.ReadLinesFromFile("input.txt");

var numbers = file.First().Split(',').Select(Int32.Parse).ToList();

var res =
    file
    .Skip(1)
    .Chunk(6)
    .Select(_ => _.Skip(1))
    .Select(_ => _.Select(_ => _.Split(' ').Where(_ => _ != "")))
    .Select(_ => _.Select(_ => _.Select(_ => (Int32.Parse(_), false))))
    .Select(_ =>
    {
        var dict = new Dictionary<int, (int, int)>();
        var bingoBoard = _.Select(_ => _.ToArray()).ToArray();
        foreach (var (item, i) in _.WithIndex())
        {
            foreach (var (val, j) in item.WithIndex())
            {
                dict[val.Item1] = (i, j);
            }
        }
        foreach ((int n, int index) in numbers.WithIndex())
        {
            if (dict.ContainsKey(n))
            {
                var pos = dict[n];
                bingoBoard[pos.Item1][pos.Item2].Item2 = true;
                if (bingoBoard.Any(line => line.All(n => n.Item2 == true))
                   || bingoBoard.Transpose().Any(line => line.All(n => n.Item2 == true)))
                {
                    return (index, n * bingoBoard.Sum(line => line.Where(i => i.Item2 == false).Select(i => i.Item1).Sum()));
                }
            }
        }
        return (int.MaxValue, -1);
    })
    .MaxBy(_ => _.Item1);

Console.WriteLine(res.Item2);
