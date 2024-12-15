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

static Dictionary<long, Dictionary<long, Item>> ParseInput2(List<string> input)
{
    var ret = new Dictionary<long, Dictionary<long, Item>>();
    var c = input[0].Length;
    for (int i = 0; i < input.Count; i++)
    {
        ret.Add(i, new Dictionary<long, Item>());
        for (int j = 0; j < c; j++)
        {
            var v = input[i][j].ToString();
            if (v == "@")
            {
                var item = new Item { Coordinate = new Coordinate { X = 2 * j, Y = i }, Tile = v };
                var item2 = new Item { Coordinate = new Coordinate { X = 2 * j + 1, Y = i }, Tile = "." };
                ret[i].Add(2 * j, item);
                ret[i].Add(2 * j + 1, item2);
            }
            else if (v == "O")
            {
                Box b = new Box { Left = new Coordinate { X = 2 * j, Y = i } , Right = new Coordinate { X = 2 * j + 1, Y = i } };
                var item = new Item { Coordinate = new Coordinate { X = 2 * j, Y = i }, Tile = v, Box = b };
                var item2 = new Item { Coordinate = new Coordinate { X = 2 * j + 1, Y = i }, Tile = v, Box = b };
                ret[i].Add(2 * j, item);
                ret[i].Add(2 * j + 1, item2);
            }
            else
            {
                var item = new Item { Coordinate = new Coordinate { X = 2 * j, Y = i }, Tile = v };
                var item2 = new Item { Coordinate = new Coordinate { X = 2 * j + 1, Y = i }, Tile = v };
                ret[i].Add(2 * j, item);
                ret[i].Add(2 * j + 1, item2);
            }
        }
    }

    return ret;
}


static bool Push(Item item, Coordinate dir, Dictionary<long, Dictionary<long, Item>> map)
{
    var t = map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X].Tile;
    if (t == "#")
        return false;
    if (t == ".")
    {
        map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X].Tile = "O";
        map[item.Coordinate.Y][item.Coordinate.X].Tile = ".";

        return true;
    }
    var ret = Push(map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X], dir, map);
    if (ret)
    {
        map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X].Tile = "O";
        map[item.Coordinate.Y][item.Coordinate.X].Tile = ".";
    }
    return ret;
}

static bool CanPush2(Box box, Coordinate dir, Dictionary<long, Dictionary<long, Item>> map)
{
    var t1 = map[box.Left.Y + dir.Y][box.Left.X + dir.X];
    var t2 = map[box.Right.Y + dir.Y][box.Right.X + dir.X];
    if (t1.Tile == "#" || t2.Tile == "#")
        return false;
    if (t1.Tile == "." && t2.Tile == ".")
    {
        return true;
    }
    if (t1.Box == box && t2.Tile == ".")
        return true;
    if (t2.Box == box && t1.Tile == ".")
        return true;
    
    var ret1 = t1.Box == null || t1.Box == box ? true : CanPush2(t1.Box, dir, map);
    var ret2 = t2.Box == null || t2.Box == box ? true : CanPush2(t2.Box, dir, map);
    return ret1 && ret2;
}

static void PushBox(Box box, Coordinate dir, Dictionary<long, Dictionary<long, Item>> map)
{
    map[box.Left.Y][box.Left.X].Box = null;
    map[box.Left.Y][box.Left.X].Tile = ".";
    map[box.Right.Y][box.Right.X].Box = null;
    map[box.Right.Y][box.Right.X].Tile = ".";

    box.Left.X += dir.X;
    box.Left.Y += dir.Y;
    var li = map[box.Left.Y][box.Left.X];
    var lb = li.Box;
    if(lb != null)
        PushBox(lb, dir, map);
    li.Box = box;
    li.Tile = "O";
    box.Right.X += dir.X;
    box.Right.Y += dir.Y;
    var ri = map[box.Right.Y][box.Right.X];
    var rb = ri.Box;
    if (rb != null && lb != rb)
    {
        PushBox(rb, dir, map);
    }
    ri.Box = box;
    ri.Tile = "O";
}

static Item Move(Item item, Coordinate dir, Dictionary<long, Dictionary<long, Item>> map)
{
    var t = map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X];
    if (t.Tile == "#")
        return item;
    if (t.Tile == ".")
    {
        map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X].Tile = "@";
        map[item.Coordinate.Y][item.Coordinate.X].Tile = ".";
        return map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X];
    }
    var p = Push(t, dir, map);
    if(!p)
    {
        return item;
    }
    else
    {
        map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X].Tile = "@";
        map[item.Coordinate.Y][item.Coordinate.X].Tile = ".";
        return map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X];
    }
}

static Item Move2(Item item, Coordinate dir, Dictionary<long, Dictionary<long, Item>> map)
{
    var t = map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X];
    if (t.Tile == "#")
        return item;
    if (t.Tile == ".")
    {
        map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X].Tile = "@";
        map[item.Coordinate.Y][item.Coordinate.X].Tile = ".";
        return map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X];
    }
    var p = CanPush2(t.Box, dir, map);
    if (!p)
    {
        return item;
    }
    else
    {
        map[item.Coordinate.Y][item.Coordinate.X].Tile = ".";
        PushBox(t.Box, dir, map);
        map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X].Tile = "@";
        return map[item.Coordinate.Y + dir.Y][item.Coordinate.X + dir.X];
    }
}

var input = File.ReadAllLines("input.txt").ToList();
var ms = input.TakeWhile(s => s != "").ToList();
var moves = string.Join("", input.SkipWhile(s => s != ""));
var map = ParseInput2(ms);
var robot = map.SelectMany(v => v.Value.Values).Single(s => s.Tile == "@");

VisualizeMap(map);

foreach (var move in moves)
{
    if (move == '^')
    {
        robot = Move2(robot, new Coordinate { X = 0, Y = -1 }, map);
    }
    else if (move == '>')
    {
        robot = Move2(robot, new Coordinate { X = 1, Y = 0 }, map);
    }
    else if (move == 'v')
    {
        robot = Move2(robot, new Coordinate { X = 0, Y = 1 }, map);
    }
    else if (move == '<')
    {
        robot = Move2(robot, new Coordinate { X = -1, Y = 0 }, map);
    }

    VisualizeMap(map);
}


//var ret = map.SelectMany(v=>v.Value.Values).Where(i=>i.Tile == "O").Sum(t=>t.Coordinate.Y * 100 + t.Coordinate.X);
var ret = map.SelectMany(v=>v.Value.Values).Where(i=>i.Box != null).Select(t=>t.Box).Distinct().Sum(t=>t.Left.Y * 100 + t.Left.X);
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
    public string Tile { get; set; }
    public Box Box { get; set; }
}

public class Box
{
    public Coordinate Left { get; set; }
    public Coordinate Right { get; set; }
}

