using System.Runtime.InteropServices;
using System.Text;

static void VisualizeMap(Dictionary<long, Dictionary<long, Item>> map)
{
    StringBuilder sb = new StringBuilder();
    
    for (int y = 0; y < map.Count; y++)
        sb.AppendLine(string.Join("", map[y].Select(k => k.Value.Tile)));

    Console.WriteLine(sb.ToString());
    //File.AppendAllText("out.txt", sb.ToString());
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

static long Diff(Coordinate c1, Coordinate c2)
{
    return Math.Abs(c1.X - c2.X) + Math.Abs(c1.Y - c2.Y);
}

static long? Diff2(Coordinate c1, Coordinate c2, Dictionary<long, Dictionary<long, Item>> map, Dictionary<long, Dictionary<long, long?>> cache)
{
    var steps = new List<(Coordinate, bool, List<Coordinate>, long, HashSet<long>, Coordinate, Dictionary<long, long>)>();
    steps.Add((c1, false, new List<Coordinate> { c1 }, 0, new HashSet<long>(), null, new Dictionary<long, long>()));
    List<Coordinate> path = null;
    var c2v = c2.Y * 100000 + c2.X;
    long? diff = null;

    if (!cache.ContainsKey(c2v))
        cache.Add(c2v, new Dictionary<long, long?>());
    bool succ = false;
    while (steps.Any())
    {
        steps = steps.OrderBy(s => s.Item4).ToList();
        var cs = steps.First();
        steps.RemoveAt(0);

        if (cs.Item1.Y == c2.Y && cs.Item1.X == c2.X)
        {
            succ = true;
            path = cs.Item3;
            diff = cs.Item4;

            //foreach(var p in path)
            //{
            //    map[p.Y][p.X].Tile = "O";
            //}
            //VisualizeMap(map);


            break;
        }

        foreach (var a in cs.Item1.Adjacent())
        {
            if (map.ContainsKey(a.Y) && map[a.Y].ContainsKey(a.X))
            {
                var cv = a.Y * 100000 + a.X;
                if (cs.Item5.Contains(cv))
                {
                    continue;
                }
                if (cache[c2v].ContainsKey(cv))
                {
                    diff = cache[c2v][cv] + cs.Item4 + 1;
                }


                if ((map[a.Y][a.X].Tile == "#" || (a.Y == c2.Y && a.X == c2.X)) && cs.Item4 + 1 <= 20)
                {
                    if (!cs.Item7.ContainsKey(cv) || cs.Item7[cv] > cs.Item4)
                    {
                        steps.Add((a, false, cs.Item3.Append(a).ToList(), cs.Item4 + 1, cs.Item5.Append(cv).ToHashSet(), cs.Item6, cs.Item7));
                        cs.Item7.Add(cv, cs.Item4);
                    }
                }
            }
        }
    }

    if (succ)
    {
        var s = 1;
        foreach (var p in path)
        {
            var cv = p.Y * 100000 + p.X;
            cache[c2v][cv] = s;
        }
        return diff;
    }
    else
    {
        return null;
    }
}


var input = File.ReadAllLines("input.txt").ToList();
var ret = 0;
var ret2 = 0;

var map = ParseInput(input);

Coordinate pos = map.SelectMany(v => v.Value.Values).Single(t => t.Tile == "S").Coordinate;
Coordinate goal = map.SelectMany(v => v.Value.Values).Single(t => t.Tile == "E").Coordinate;

var steps = new List<(Coordinate, bool, List<Coordinate>, long, HashSet<long>, Coordinate)>();
steps.Add((pos, false, new List<Coordinate> { pos }, 0, new HashSet<long>(), null));

List<Coordinate> path = new List<Coordinate>();

while (steps.Any())
{
    var cs = steps.First();
    steps.RemoveAt(0);

    if (cs.Item1.Y == goal.Y && cs.Item1.X == goal.X)
    {
        path = cs.Item3;
        break;
    }

    foreach (var a in cs.Item1.Adjacent())
    {
        if (map.ContainsKey(a.Y) && map[a.Y].ContainsKey(a.X))
        {
            var cv = a.Y * 100000 + a.X;
            if (cs.Item5.Contains(cv))
            {
                continue;
            }

            if (map[a.Y][a.X].Tile != "#")
            {
                steps.Add((a, false, cs.Item3.Append(a).ToList(), cs.Item4 > 0 ? 3 : 0, cs.Item5.Append(cv).ToHashSet(), cs.Item6));
            }
        }
    }
}

Dictionary<long, Dictionary<long, Coordinate>> cache = new Dictionary<long, Dictionary<long, Coordinate>>();
foreach(var c in path)
{
    if (!cache.ContainsKey(c.Y))
        cache.Add(c.Y, new Dictionary<long, Coordinate>());
    cache[c.Y].Add(c.X, c);
}

int step = 0;
int diff = 100;
Dictionary<long, Dictionary<long, long?>> cache2 = new Dictionary<long, Dictionary<long, long?>>();

foreach (var c in path)
{
    foreach(var c2 in path.Skip(100))
    {
        var d = Diff(c, c2);
        if (d <= 20)
        {
            //var rd = Diff2(c, c2, map, cache2);
            if (step + path.Count - path.IndexOf(cache[c2.Y][c2.X]) + d <= path.Count - diff)
            //if (step + path.Count - path.IndexOf(cache[c2.Y][c2.X]) + d == path.Count - diff)
            {
                ret2++;
            }
        }
    }

    //if (cache.ContainsKey(c.Y - 2) && cache[c.Y - 2].ContainsKey(c.X))
    //{
    //    if(step + (path.Count - path.IndexOf(cache[c.Y - 2][c.X])) + 1 <= path.Count - diff)
    //    {
    //        ret++;
    //    }
    //}
    //if (cache.ContainsKey(c.Y + 2) && cache[c.Y + 2].ContainsKey(c.X))
    //{
    //    if (step + (path.Count - path.IndexOf(cache[c.Y + 2][c.X])) + 1 <= path.Count - diff)
    //    {
    //        ret++;
    //    }
    //}
    //if (cache.ContainsKey(c.Y) && cache[c.Y].ContainsKey(c.X - 2))
    //{
    //    if (step + (path.Count - path.IndexOf(cache[c.Y][c.X - 2])) + 1 <= path.Count - diff)
    //    {
    //        ret++;
    //    }
    //}
    //if (cache.ContainsKey(c.Y) && cache[c.Y].ContainsKey(c.X + 2))
    //{
    //    if (step + (path.Count - path.IndexOf(cache[c.Y][c.X + 2])) + 1 <= path.Count - diff)
    //    {
    //        ret++;
    //    }
    //}
    step++;
}

Console.WriteLine(ret2);
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