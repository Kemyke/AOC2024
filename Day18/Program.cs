using System.Text;

static void VisualizeMap(Dictionary<long, Dictionary<long, Item>> map)
{
    StringBuilder sb = new StringBuilder();

    for (int y = -1; y < map.Count - 1; y++)
        sb.AppendLine(string.Join("", map[y].OrderBy(t=>t.Key).Select(k => k.Value.Tile)));

    Console.WriteLine(sb.ToString());
    //File.AppendAllText("out.txt", sb.ToString());
}

static Dictionary<long, Dictionary<long, Item>> ParseInput(List<string> input, int size, int bytes)
{
    var ret = new Dictionary<long, Dictionary<long, Item>>();

    for (int y = 0; y < size; y++)
    {
        ret.Add(y, new Dictionary<long, Item>());
        for (int x = 0; x < size; x++)
        {
            var item = new Item { Coordinate = new Coordinate { X = x, Y = y }, Tile = "." };
            ret[y].Add(x, item);
        }
    }

    ret.Add(-1, new Dictionary<long, Item>());
    ret.Add(size, new Dictionary<long, Item>());

    for (int i = -1; i < size + 1; i++)
    {

        var item = new Item { Coordinate = new Coordinate { X = i, Y = -1 }, Tile = "#" };
        ret[-1][i] = item;

        var item2 = new Item { Coordinate = new Coordinate { X = i, Y = size }, Tile = "#" };
        ret[size].Add(i, item2);

        var item3 = new Item { Coordinate = new Coordinate { X = -1, Y = i }, Tile = "#" };
        ret[i][-1] = item3;

        var item4 = new Item { Coordinate = new Coordinate { X = size, Y = i }, Tile = "#" };
        ret[i][size] = item4;
    }

    foreach (var line in input.Take(bytes))
    {
        var ss = line.Split(",");
        ret[int.Parse(ss[1])][int.Parse(ss[0])].Tile = "#";
    }
    Console.WriteLine(input.Take(bytes).Last());

    return ret;
}

static long GetHeur(Coordinate pos, Coordinate end)
{
    var ret = Math.Abs(pos.X - end.X) + Math.Abs(pos.Y - end.Y);
    return ret;
}


var input = File.ReadAllLines("input.txt").ToList();
var map = ParseInput(input, 71, 1024);
long ret = 0;

int bytes = 2800;

while (true)
{
    map = ParseInput(input, 71, bytes);

    Coordinate pos = new Coordinate { X = 0, Y = 0 };
    Coordinate goal = new Coordinate { X = 70, Y = 70 };

    var cache = new Dictionary<long, Dictionary<long, long>>();

    //VisualizeMap(map);

    for (int y = 0; y < map.Count; y++)
    {
        cache.Add(y, new Dictionary<long, long>());
        for (int x = 0; x < map.First().Value.Count; x++)
        {
            cache[y].Add(x, long.MaxValue);
        }
    }


    var steps = new List<(Coordinate, long, List<Coordinate>, long, HashSet<long>)>();
    steps.Add((pos, 0, new List<Coordinate> { pos }, 0, new HashSet<long>()));

    while (steps.Any())
    {
        steps = steps.OrderBy(s => s.Item4).ToList();
        var cs = steps.First();
        steps.RemoveAt(0);

        if (cs.Item1.Y == goal.Y && cs.Item1.X == goal.X)
        {
            ret = cs.Item3.Count;
            break;
        }

        foreach (var a in cs.Item1.Adjacent())
        {
            if (map[a.Y][a.X].Tile != "#")
            {
                var cv = a.Y * 100000 + a.X;
                if (cs.Item5.Contains(cv))
                {
                    continue;
                }

                var c = 1;
                var nc = cs.Item2 + c;
                var nh = nc + GetHeur(a, goal);
                if (cache[a.Y][a.X] > nh)
                {
                    steps.Add((a, nc, cs.Item3.Append(a).ToList(), nh, cs.Item5.Append(cv).ToHashSet()));
                    cache[a.Y][a.X] = nh;
                }
            }
        }
    }
    if (!steps.Any())
        break;
    bytes++;
}
Console.WriteLine(ret);
Console.ReadLine();

public class Coordinate
{
    public long X { get; set; }
    public long Y { get; set; }

    public List<Coordinate> Adjacent()
    {
        List<Coordinate> ret =
        [
            new Coordinate { Y = Y - 1, X = X },
            new Coordinate { Y = Y, X = X - 1},
            new Coordinate { Y = Y, X = X + 1 },
            new Coordinate { Y = Y + 1, X = X },
        ];

        return ret;
    }
}

public class Item
{
    public Coordinate Coordinate { get; set; }
    public string Tile { get; set; }
}