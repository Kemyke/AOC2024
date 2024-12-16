using System.Text;

static void VisualizeMap(Dictionary<long, Dictionary<long, Item>> map)
{
    StringBuilder sb = new StringBuilder();

    for (int y = 0; y < map.Count; y++)
        sb.AppendLine(string.Join("", map[y].Select(k => k.Value.Tile)));

    File.AppendAllText("out.txt", sb.ToString());
}

static Dictionary<long, Dictionary<long, Item>> ParseInput(List<string> input)
{
    var ret = new Dictionary<long, Dictionary<long, Item>>();
    var c = input[0].Length;
    for (int i = 0; i < input.Count; i++)
    {
        ret.Add(i, new Dictionary<long, Item>());
        for (int j = 0; j < c; j++)
        {
            var item = new Item { Coordinate = new Coordinate { X = j, Y = i }, Tile = input[i][j].ToString() };
            ret[i].Add(j, item);
        }
    }

    return ret;
}

static long GetHeur(Coordinate pos, string face, Coordinate end)
{
    var ret = Math.Abs(pos.X - end.X) + Math.Abs(pos.Y - end.Y);
    if (face == "N" || face == "E")
    {
        if ((face == "N" && pos.X == end.X) || (face == "E" && pos.Y == end.Y))
        {

        }
        else
        {
            ret += 1000;
        }
    }
    if (face == "S" || face == "W")
        ret += 2000;
    return ret;
}

var input = File.ReadAllLines("input.txt").ToList();
var map = ParseInput(input);

Coordinate pos = map.SelectMany(t => t.Value.Values).Single(i => i.Tile == "S").Coordinate;
Coordinate goal = map.SelectMany(t => t.Value.Values).Single(i => i.Tile == "E").Coordinate;
List<(List<Coordinate>, long)> ret2 = new List<(List<Coordinate>, long)>();

var cache = new Dictionary<long, Dictionary<long, Dictionary<string, long>>>();

for (int y = 0; y < map.Count; y++)
{
    cache.Add(y, new Dictionary<long, Dictionary<string, long>>());
    for (int x = 0; x < map.First().Value.Count; x++)
    {
        cache[y].Add(x, new Dictionary<string, long>());
        cache[y][x].Add("W", long.MaxValue);
        cache[y][x].Add("E", long.MaxValue);
        cache[y][x].Add("N", long.MaxValue);
        cache[y][x].Add("S", long.MaxValue);
    }
}
var steps = new List<(Coordinate, string, long, List<Coordinate>, long, HashSet<long>)>();
steps.Add((pos, "H", 0, new List<Coordinate> { pos }, 0, new HashSet<long>()));

VisualizeMap(map);

while(steps.Any())
{
    steps = steps.OrderBy(s => s.Item5).ToList();
    var cs = steps.First();
    steps.RemoveAt(0);

    if (map[cs.Item1.Y][cs.Item1.X].Tile == "E")
    {
        ret2.Add((cs.Item4, cs.Item3));
    }

    foreach (var a in cs.Item1.Adjacent())
    {
        if (map[a.Item1.Y][a.Item1.X].Tile != "#")
        {
            var cv = a.Item1.Y * 100000 + a.Item1.X;
            if (cs.Item6.Contains(cv))
            {
                continue;
            }

            var c = 1;
            var face = cs.Item2;
            var last = cs.Item4.Last();
            if (last.Y == a.Item1.Y)
            {
                if (cs.Item2 == "V")
                {
                    c += 1000;
                    face = "H";
                }
            }
            if (last.X == a.Item1.X)
            {
                if (cs.Item2 == "H")
                {
                    c += 1000;
                    face = "V";
                }
            }

            var nc = cs.Item3 + c;
            var nh = nc + GetHeur(a.Item1, a.Item2, goal);
            if (cache[a.Item1.Y][a.Item1.X][a.Item2] >= nh)
            {
                steps.Add((a.Item1, face, nc, cs.Item4.Append(a.Item1).ToList(), nh, cs.Item6.Append(cv).ToHashSet()));
                cache[a.Item1.Y][a.Item1.X][a.Item2] = nh;
            }
        }
    }
    
}

var ret = ret2.Min(p => p.Item2);

foreach (var cc in ret2.Where(c => c.Item2 == ret))
    foreach (var c in cc.Item1)
    {
        map[c.Y][c.X].Tile = "O";
    }
VisualizeMap(map);

Console.WriteLine(ret);
Console.WriteLine(map.SelectMany(m=>m.Value.Values).Count(x=>x.Tile == "O"));
Console.ReadLine();

public class Coordinate
{
    public long X { get; set; }
    public long Y { get; set; }

    public List<(Coordinate, string)> Adjacent()
    {
        List<(Coordinate, string)> ret =
        [
            (new Coordinate { Y = Y - 1, X = X }, "N"),
            (new Coordinate { Y = Y, X = X - 1}, "W"),
            (new Coordinate { Y = Y, X = X + 1 }, "E"),
            (new Coordinate { Y = Y + 1, X = X }, "S"),
        ];

        return ret;
    }
}

public class Item
{
    public Coordinate Coordinate { get; set; }
    public string Tile { get; set; }
}
