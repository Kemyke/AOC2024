static Dictionary<long, Dictionary<long, Item>> ParseInput(List<string> input)
{
    var ret = new Dictionary<long, Dictionary<long, Item>>();
    var c = input[0].Length;
    for (int i = 0; i < input.Count; i++)
    {
        ret.Add(i, new Dictionary<long, Item>());
        for (int j = 0; j < c; j++)
        {
            var item = new Item { Coordinate = new Coordinate { X = j, Y = i }, ValueCh = input[i][j] };
            ret[i].Add(j, item);
        }
    }

    return ret;
}

static (Coordinate, Coordinate) AntinodeLocations(Coordinate f1, Coordinate f2)
{
    var dx = Math.Abs(f1.X - f2.X);
    var dy = Math.Abs(f1.Y - f2.Y);

    Coordinate r1 = new Coordinate { X = (f1.X > f2.X) ? f1.X + dx : f1.X - dx, Y = (f1.Y > f2.Y) ? f1.Y + dy : f1.Y - dy };
    Coordinate r2 = new Coordinate { X = (f2.X > f1.X) ? f2.X + dx : f2.X - dx, Y = (f2.Y > f1.Y) ? f2.Y + dy : f2.Y - dy };

    return (r1, r2);
}

static List<Coordinate> AntinodeLocations2(int maxx, int maxy, Coordinate f1, Coordinate f2)
{
    var dx = Math.Abs(f1.X - f2.X);
    var dy = Math.Abs(f1.Y - f2.Y);

    List<Coordinate> ret = new List<Coordinate>();

    Coordinate r1 = new Coordinate { X = (f1.X > f2.X) ? f1.X + dx : f1.X - dx, Y = (f1.Y > f2.Y) ? f1.Y + dy : f1.Y - dy };
    ret.Add(r1);
    int i = 1;
    while (r1.X >= 0 && r1.X <= maxx && r1.Y >= 0 && r1.Y <= maxy)
    {
        i++;
        r1 = new Coordinate { X = (f1.X > f2.X) ? f1.X + i * dx : f1.X - i * dx, Y = (f1.Y > f2.Y) ? f1.Y + i * dy : f1.Y - i * dy };
        ret.Add(r1);
    }

    Coordinate r2 = new Coordinate { X = (f2.X > f1.X) ? f2.X + dx : f2.X - dx, Y = (f2.Y > f1.Y) ? f2.Y + dy : f2.Y - dy };
    ret.Add(r2);
    i = 1;
    while (r2.X >= 0 && r2.X <= maxx && r2.Y >= 0 && r2.Y <= maxy)
    {
        i++;
        r2 = new Coordinate { X = (f2.X > f1.X) ? f2.X + i * dx : f2.X - i * dx, Y = (f2.Y > f1.Y) ? f2.Y + i * dy : f2.Y - i * dy };
        ret.Add(r2);
    }
    return ret;
}

var input = File.ReadAllLines("input.txt").ToList();
var map = ParseInput(input);

var fls = map.Values.SelectMany(s => s.Values).Where(i => i.ValueCh != '.').ToList();
var freqs = fls.Select(i=>i.ValueCh).Distinct().ToList();


foreach(var f in fls)
{
    f.Antinode = true;
}

var maxx = map.First().Value.Count;
var maxy = map.Count;
foreach (var freq in freqs)
{
    var fl = fls.Where(f=>f.ValueCh == freq).ToList();
    for(int i=0;i<fl.Count;i++)
        for(int j=i+1;j<fl.Count;j++)
        {
            //Part 1
            //var ans = AntinodeLocations(fl[i].Coordinate, fl[j].Coordinate);
            //if(map.ContainsKey(ans.Item1.Y) && map[ans.Item1.Y].ContainsKey(ans.Item1.X))
            //{
            //    map[ans.Item1.Y][ans.Item1.X].Antinode = true;
            //}
            //if (map.ContainsKey(ans.Item2.Y) && map[ans.Item2.Y].ContainsKey(ans.Item2.X))
            //{
            //    map[ans.Item2.Y][ans.Item2.X].Antinode = true;
            //}

            var ans = AntinodeLocations2(maxx, maxy, fl[i].Coordinate, fl[j].Coordinate);
            foreach(var an in ans)
            {
                if (map.ContainsKey(an.Y) && map[an.Y].ContainsKey(an.X))
                {
                    map[an.Y][an.X].Antinode = true;
                }

            }
        }
}

var ret = map.Values.SelectMany(s => s.Values).Count(i=>i.Antinode);

Console.WriteLine(ret);
Console.ReadLine();

public class Coordinate
{
    public long X { get; set; }
    public long Y { get; set; }
}

public class Item
{
    public Coordinate Coordinate { get; set; }
    public char ValueCh { get; set; }
    public bool Antinode { get; set; } = false;
}
