static Dictionary<long, Dictionary<long, Item>> ParseInput(List<string> input)
{
    var ret = new Dictionary<long, Dictionary<long, Item>>();
    var c = input[0].Length;
    for (int i = 0; i < input.Count; i++)
    {
        ret.Add(i, new Dictionary<long, Item>());
        for (int j = 0; j < c; j++)
        {
            var item = new Item { Coordinate = new Coordinate { X = j, Y = i }, PlantType = input[i][j] };
            ret[i].Add(j, item);
        }
    }

    return ret;
}

static void FillArea(Dictionary<long, Dictionary<long, Item>> map, Item item, Region r)
{
    foreach (var a in item.Coordinate.Adjacent())
    {
        if (map.ContainsKey(a.Y) && map[a.Y].ContainsKey(a.X))
        {
            var ai = map[a.Y][a.X];
            if (ai.PlantType == item.PlantType && ai.Region == null)
            {
                ai.Region = r;
                r.Area++;
                FillArea(map, ai, r);
                if (!r.Coordinates.ContainsKey(ai.Coordinate.Y))
                    r.Coordinates.Add(ai.Coordinate.Y, new Dictionary<long, Coordinate>());
                r.Coordinates[ai.Coordinate.Y].Add(ai.Coordinate.X, ai.Coordinate);

            }
        }
    }
}

static void CalcPerimeter(Region r)
{
    var p = 0;
    foreach(var c in r.Coordinates.Values.SelectMany(v=>v.Values))
    {
        foreach(var cc in c.Adjacent())
        {
            if(!r.Coordinates.ContainsKey(cc.Y) || !r.Coordinates[cc.Y].ContainsKey(cc.X))
            {
                p++;
            }
        }
    }
    r.Perimeter = p;
}

static void CalcPerimeter2(Region r)
{
    var upperHlines = new Dictionary<long, List<HLine>>();
    var bottomHlines = new Dictionary<long, List<HLine>>();
    var leftVlines = new Dictionary<long, List<VLine>>();
    var rightVlines = new Dictionary<long, List<VLine>>();

    var p = 0;
    foreach (var c in r.Coordinates.Values.SelectMany(v => v.Values))
    {
        if (!r.Coordinates.ContainsKey(c.Y - 1) || !r.Coordinates[c.Y - 1].ContainsKey(c.X))
        {
            if(!upperHlines.ContainsKey(c.Y))
            {
                upperHlines.Add(c.Y, new List<HLine>());
            }

            var n1 = upperHlines[c.Y].SingleOrDefault(l => c.X == l.EndX + 1);
            var n2 = upperHlines[c.Y].SingleOrDefault(l => c.X == l.StartX - 1);
            if(n1 != null && n2 != null)
            {
                upperHlines[c.Y].Remove(n1);
                upperHlines[c.Y].Remove(n2);
                upperHlines[c.Y].Add(new HLine { Y = c.Y, StartX = Math.Min(n1.StartX, n2.StartX), EndX = Math.Max(n1.EndX, n2.EndX) });
            }
            else if(n1 != null)
            {
                n1.EndX++;
            }
            else if (n2 != null)
            {
                n2.StartX--;
            }
            else
            {
                upperHlines[c.Y].Add(new HLine { Y = c.Y, StartX = c.X, EndX = c.X });
            }
        } 
        if (!r.Coordinates.ContainsKey(c.Y + 1) || !r.Coordinates[c.Y + 1].ContainsKey(c.X))
        {
            if (!bottomHlines.ContainsKey(c.Y))
            {
                bottomHlines.Add(c.Y, new List<HLine>());
            }

            var n1 = bottomHlines[c.Y].SingleOrDefault(l => c.X == l.EndX + 1);
            var n2 = bottomHlines[c.Y].SingleOrDefault(l => c.X == l.StartX - 1);
            if (n1 != null && n2 != null)
            {
                bottomHlines[c.Y].Remove(n1);
                bottomHlines[c.Y].Remove(n2);
                bottomHlines[c.Y].Add(new HLine { Y = c.Y, StartX = Math.Min(n1.StartX, n2.StartX), EndX = Math.Max(n1.EndX, n2.EndX) });
            }
            else if (n1 != null)
            {
                n1.EndX++;
            }
            else if (n2 != null)
            {
                n2.StartX--;
            }
            else
            {
                bottomHlines[c.Y].Add(new HLine { Y = c.Y, StartX = c.X, EndX = c.X });
            }
        }
        if (!r.Coordinates.ContainsKey(c.Y) || !r.Coordinates[c.Y].ContainsKey(c.X - 1))
        {
            if (!leftVlines.ContainsKey(c.X))
            {
                leftVlines.Add(c.X, new List<VLine>());
            }

            var n1 = leftVlines[c.X].SingleOrDefault(l => c.Y == l.EndY + 1);
            var n2 = leftVlines[c.X].SingleOrDefault(l => c.Y == l.StartY - 1);
            if (n1 != null && n2 != null)
            {
                leftVlines[c.X].Remove(n1);
                leftVlines[c.X].Remove(n2);
                leftVlines[c.X].Add(new VLine { X = c.X, StartY = Math.Min(n1.StartY, n2.StartY), EndY = Math.Max(n1.EndY, n2.EndY) });
            }
            else if (n1 != null)
            {
                n1.EndY++;
            }
            else if (n2 != null)
            {
                n2.StartY--;
            }
            else
            {
                leftVlines[c.X].Add(new VLine { X = c.X, StartY = c.Y, EndY = c.Y });
            }
        }
        if (!r.Coordinates.ContainsKey(c.Y) || !r.Coordinates[c.Y].ContainsKey(c.X + 1))
        {
            if (!rightVlines.ContainsKey(c.X))
            {
                rightVlines.Add(c.X, new List<VLine>());
            }

            var n1 = rightVlines[c.X].SingleOrDefault(l => c.Y == l.EndY + 1);
            var n2 = rightVlines[c.X].SingleOrDefault(l => c.Y == l.StartY - 1);
            if (n1 != null && n2 != null)
            {
                rightVlines[c.X].Remove(n1);
                rightVlines[c.X].Remove(n2);
                rightVlines[c.X].Add(new VLine { X = c.X, StartY = Math.Min(n1.StartY, n2.StartY), EndY = Math.Max(n1.EndY, n2.EndY) });
            }
            else if (n1 != null)
            {
                n1.EndY++;
            }
            else if (n2 != null)
            {
                n2.StartY--;
            }
            else
            {
                rightVlines[c.X].Add(new VLine { X = c.X, StartY = c.Y, EndY = c.Y });
            }
        }
    }
    r.Perimeter2 = upperHlines.Values.Sum(v=>v.Count) + bottomHlines.Values.Sum(v => v.Count) + leftVlines.Values.Sum(v => v.Count) + rightVlines.Values.Sum(v => v.Count);
}

var input = File.ReadAllLines("input.txt").ToList();
var map = ParseInput(input);
var ret1 = 0;
var ret2 = 0;

foreach(var item in map.Values.SelectMany(v=>v.Values))
{
    if (item.Region == null)
    {
        item.Region = new Region { PlantType = item.PlantType, Area = 1 };
        item.Region.Coordinates.Add(item.Coordinate.Y, new Dictionary<long, Coordinate>());
        item.Region.Coordinates[item.Coordinate.Y].Add(item.Coordinate.X, item.Coordinate);
        FillArea(map, item, item.Region);
    }
}

var regions = map.Values.SelectMany(v => v.Values).Select(i => i.Region).Distinct();
foreach(var r in regions)
{
    CalcPerimeter(r);
    CalcPerimeter2(r);

    ret1 += r.Perimeter * r.Area;
    ret2 += r.Perimeter2 * r.Area;
}


Console.WriteLine(ret1);
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
                new Coordinate { Y = Y, X = X - 1 },
                new Coordinate { Y = Y, X = X + 1 },
                new Coordinate { Y = Y + 1, X = X },
            ];

        return ret;
    }
}

public class Item
{
    public Coordinate Coordinate { get; set; }
    public char PlantType { get; set; }
    public Region Region { get; set; }
}

public class Region
{
    public int Perimeter { get; set; }
    public int Perimeter2 { get; set; }
    public int Area { get; set; }
    public char PlantType { get; set; }
    public Dictionary<long, Dictionary<long, Coordinate>> Coordinates { get; set; } = new Dictionary<long, Dictionary<long, Coordinate>>();
}

public class HLine
{
    public long Y { get; set; }
    public long StartX { get; set; }
    public long EndX { get; set; }
}

public class VLine
{
    public long X { get; set; }
    public long StartY { get; set; }
    public long EndY { get; set; }
}