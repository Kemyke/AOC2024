static Dictionary<long, Dictionary<long, Item>> Clone(Dictionary<long, Dictionary<long, Item>> map)
{
    Dictionary<long, Dictionary<long, Item>> ret = new Dictionary<long, Dictionary<long, Item>>(); 
    foreach (var y in map)
    {
        ret.Add(y.Key, new Dictionary<long, Item>());
        foreach (var x in y.Value)
        {
            ret[y.Key].Add(x.Key, x.Value.Clone());
        }
    }
    return ret;
}

static (Item, Dictionary<long, Dictionary<long, Item>>) ParseInput(List<string> input)
{
    Item currentP = null; 
    var ret = new Dictionary<long, Dictionary<long, Item>>();
    var c = input[0].Length;
    for (int i = 0; i < input.Count; i++)
    {
        ret.Add(i, new Dictionary<long, Item>());
        for (int j = 0; j < c; j++)
        {
            var item = new Item { Coordinate = new Coordinate { X = j, Y = i }, ValueCh = input[i][j] };
            ret[i].Add(j, item);

            if (item.ValueCh == '^')
                currentP = item;
        }
    }

    return (currentP, ret);
}

static (bool, Item, char) Step(Item currentP, char dir, Dictionary<long, Dictionary<long, Item>> map)
{
    if(dir == 'N')
    {
        var nexty = currentP.Coordinate.Y - 1;
        if (nexty < 0)
            return (true, null, ' ');
        Item nextP = map[nexty][currentP.Coordinate.X];

        if(nextP.ValueCh == '#')
        {
            return (false, currentP, 'E');
        }
        nextP.Visited = true;
        currentP.Stepped.Add('N');
        return (false, nextP, 'N');
    }
    if (dir ==  'E')
    {
        var nextx = currentP.Coordinate.X + 1;
        if (nextx >= map.Values.Count)
            return (true, null, ' ');
        Item nextP = map[currentP.Coordinate.Y][currentP.Coordinate.X + 1];

        if (nextP.ValueCh == '#')
        {
            return (false, currentP, 'S');
        }

        nextP.Visited = true;
        currentP.Stepped.Add('E');
        return (false, nextP, 'E');
    }
    if (dir == 'S')
    {
        var nexty = currentP.Coordinate.Y + 1;
        if (nexty >= map.Count)
            return (true, null, ' ');
        Item nextP = map[currentP.Coordinate.Y + 1][currentP.Coordinate.X];

        if (nextP.ValueCh == '#')
        {
            return (false, currentP, 'W');
        }

        nextP.Visited = true;
        currentP.Stepped.Add('S');

        return (false, nextP, 'S');
    }
    if (dir == 'W')
    {
        var nextx = currentP.Coordinate.X - 1;
        if (nextx < 0)
            return (true, null, ' ');
        Item nextP = map[currentP.Coordinate.Y][currentP.Coordinate.X - 1];

        if (nextP.ValueCh == '#')
        {
            return (false, currentP, 'N');
        }

        nextP.Visited = true;
        currentP.Stepped.Add('W');

        return (false, nextP, 'W');
    }
    return (false, null, ' ');
}

var input = File.ReadAllLines("input.txt");
var (currentP, map) = ParseInput(input.ToList());

var ret1 = 0;
var ret2 = 0;

var map1 = Clone(map);

var c1p = map1[currentP.Coordinate.Y][currentP.Coordinate.X];
c1p.Visited = true;
(bool, Item, char) c = (false, c1p, 'N');

while (c.Item1 != true)
{
    c = Step(c.Item2, c.Item3, map1);
}
ret1 = map1.Values.SelectMany(v => v.Values).Count(i => i.Visited);
Console.WriteLine(ret1);

foreach(var item in map1.Values.SelectMany(v => v.Values).Where(i => i.Visited))
{
    if (item.Coordinate.Y == currentP.Coordinate.Y && item.Coordinate.X == currentP.Coordinate.X)
        continue;

    var map2 = Clone(map);
    map2[item.Coordinate.Y][item.Coordinate.X].ValueCh = '#';
    var c2p = map2[currentP.Coordinate.Y][currentP.Coordinate.X];
    c2p.Visited = true;
    c = (false, c2p, 'N');

    while (c.Item1 != true && !c.Item2.Stepped.Contains(c.Item3))
    {
        c = Step(c.Item2, c.Item3, map2);
    }
    if (c.Item2 != null && c.Item2.Stepped.Contains(c.Item3))
        ret2++;
}

Console.WriteLine(ret2);
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
    public bool Visited { get; set; } = false;
    public List<char> Stepped { get; set; } = new List<char>();

    public Item Clone()
    {
        return new Item { Coordinate = new Coordinate { X = Coordinate.X, Y = Coordinate.Y }, ValueCh = ValueCh };
    }
}

