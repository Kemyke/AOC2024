using System.Reflection.Metadata;
using System.Text;

static void VisualizeMap(Dictionary<long, Dictionary<long, Item>> map)
{
    StringBuilder sb = new StringBuilder();

    for (int y = 0; y < map.Count; y++)
        sb.AppendLine(string.Join("", map[y].Select(k => k.Value.Tile)));

    //File.AppendAllText("out.txt", sb.ToString());
    Console.Write(sb.ToString());
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

//static long GetHeur(Coordinate pos, string face, Coordinate end)
//{
//    var ret = Math.Abs(pos.X - end.X) + Math.Abs(pos.Y - end.Y);
//    if (face == "N" || face == "E")
//        ret += 1000;
//    if (face == "S" || face == "W")
//        ret += 2000;
//    return ret;
//}

var input = File.ReadAllLines("input.txt").ToList();
var map = ParseInput(input);

string face = "E";
Coordinate pos = map.SelectMany(t => t.Value.Values).Single(i => i.Tile == "S").Coordinate;
Coordinate goal = map.SelectMany(t => t.Value.Values).Single(i => i.Tile == "E").Coordinate;
long ret = 0;
List<Coordinate> ret2 = new List<Coordinate>();

var cache = new Dictionary<long, Dictionary<long, long>>();
for(int y = 0; y < map.Count; y++)
{
    cache.Add(y, new Dictionary<long, long>());
    for (int x = 0; x < map.First().Value.Count; x++)
    {
        cache[y].Add(x, long.MaxValue);
    }
}
var steps = new List<(Coordinate, string, long, List<Coordinate>, long)>();
steps.Add((pos, face, 0, new List<Coordinate>(), 0));
//steps.Add((pos, face, 0, new List<Coordinate>(), GetHeur(pos, face, goal)));

VisualizeMap(map);

while(steps.Any())
{
    steps = steps.OrderBy(s => s.Item5).ToList();
    var cs = steps.First();
    steps.RemoveAt(0);

    if (cs.Item1.Y == 8 && cs.Item1.X == 15)
        //if (cs.Item1.Y == 13 && cs.Item1.X == 5)
    {


    }

    if(map[cs.Item1.Y][cs.Item1.X].Tile == "E")
    {
        if (ret == 0)
        {
            ret = cs.Item3;
            ret2.AddRange(cs.Item4);
            ret2 = ret2.Distinct().ToList();
            continue;
        }
        else if (cs.Item3 == ret)
        {
            ret2.AddRange(cs.Item4);
            ret2 = ret2.Distinct().ToList();
            continue;
        }
        else 
        {
            break;
        }
    }

    foreach (var a in cs.Item1.Adjacent())
    {
        if (map[a.Item1.Y][a.Item1.X].Tile != "#")
        {
            long c = 0;
            if(a.Item2 == "N")
            {
                if(cs.Item2 == "N")
                {
                    c = cs.Item3 + 1;
                }
                else if (cs.Item2 == "E" || cs.Item2 == "W")
                {
                    c = cs.Item3 + 1 + 1000;
                }
                else if (cs.Item2 == "S")
                {
                    c = cs.Item3 + 1 + 2000;
                }
            }
            else if (a.Item2 == "E")
            {
                if (cs.Item2 == "E")
                {
                    c = cs.Item3 + 1;
                }
                else if (cs.Item2 == "N" || cs.Item2 == "S")
                {
                    c = cs.Item3 + 1 + 1000;
                }
                else if (cs.Item2 == "W")
                {
                    c = cs.Item3 + 1 + 2000;
                }
            }
            if (a.Item2 == "S")
            {
                if (cs.Item2 == "S")
                {
                    c = cs.Item3 + 1;
                }
                else if (cs.Item2 == "E" || cs.Item2 == "W")
                {
                    c = cs.Item3 + 1 + 1000;
                }
                else if (cs.Item2 == "N")
                {
                    c = cs.Item3 + 1 + 2000;
                }
            }
            if (a.Item2 == "W")
            {
                if (cs.Item2 == "W")
                {
                    c = cs.Item3 + 1;
                }
                else if (cs.Item2 == "N" || cs.Item2 == "S")
                {
                    c = cs.Item3 + 1 + 1000;
                }
                else if (cs.Item2 == "E")
                {
                    c = cs.Item3 + 1 + 2000;
                }
            }
            var hc = c;//GetHeur(a.Item1, a.Item2, goal) + c;
            if (cache[a.Item1.Y][a.Item1.X] == hc)
            {

            }
            if (cache[a.Item1.Y][a.Item1.X] >= hc)
            {
                steps.Add((a.Item1, a.Item2, c, cs.Item4.Append(a.Item1).ToList(), hc));               
                cache[a.Item1.Y][a.Item1.X] = hc;
            }
        }
    }
    
}

Console.WriteLine(ret);
Console.WriteLine(ret2.Count);
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
