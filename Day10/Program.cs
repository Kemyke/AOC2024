static Dictionary<long, Dictionary<long, Item>> ParseInput(List<string> input)
{
    var ret = new Dictionary<long, Dictionary<long, Item>>();
    var c = input[0].Length;
    for (int i = 0; i < input.Count; i++)
    {
        ret.Add(i, new Dictionary<long, Item>());
        for (int j = 0; j < c; j++)
        {
            var item = new Item { Coordinate = new Coordinate { X = j, Y = i }, Height = int.Parse(input[i][j].ToString()) };
            ret[i].Add(j, item);
        }
    }

    return ret;
}

static int TrailheadScore(Dictionary<long, Dictionary<long, Item>>  map, Item trailhead)
{
    var ret = new HashSet<Item>();
    var ret2 = 0;
    var currentHeight = 0;
    var currPos = trailhead;
    var posNextSteps = new Stack<Item>();
    posNextSteps.Push(trailhead);

    while(posNextSteps.Any())
    {
        var c = posNextSteps.Pop();
        if (c.Height == 9)
        {
            ret2++;
            if (!ret.Contains(c))
                ret.Add(c);
            continue;
        }
        if(c.Coordinate.Y > 0 && map[c.Coordinate.Y - 1][c.Coordinate.X].Height == c.Height + 1)
        {
            posNextSteps.Push(map[c.Coordinate.Y - 1][c.Coordinate.X]);
        }
        if (c.Coordinate.Y < map.Count - 1 && map[c.Coordinate.Y + 1][c.Coordinate.X].Height == c.Height + 1)
        {
            posNextSteps.Push(map[c.Coordinate.Y + 1][c.Coordinate.X]);
        }
        if (c.Coordinate.X > 0 && map[c.Coordinate.Y][c.Coordinate.X - 1].Height == c.Height + 1)
        {
            posNextSteps.Push(map[c.Coordinate.Y][c.Coordinate.X - 1]);
        }
        if (c.Coordinate.X < map.First().Value.Count - 1 && map[c.Coordinate.Y][c.Coordinate.X + 1].Height == c.Height + 1)
        {
            posNextSteps.Push(map[c.Coordinate.Y][c.Coordinate.X + 1]);
        }
    }

    return ret2;
    //return ret.Count; part1
}

var input = File.ReadAllLines("input.txt").ToList();
var map = ParseInput(input);
var ret = 0;

var ths = map.SelectMany(m => m.Value.Values).Where(i => i.Height == 0).ToList();
foreach(var th in ths)
{
    ret += TrailheadScore(map, th);
}


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
    public int Height { get; set; }
}
