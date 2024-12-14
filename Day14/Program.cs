using System.Text;

static void SimulateRobot(Robot r, Coordinate mapSize, int iterNum, Dictionary<int, Dictionary<int, int>> map)
{
    for(int i =0;i<iterNum;i++)
    {
        map[r.Pos.Y][r.Pos.X]--;
        r.Pos.X = (r.Pos.X + r.V.X) % mapSize.X;
        if (r.Pos.X < 0) r.Pos.X += mapSize.X;
        r.Pos.Y = (r.Pos.Y + r.V.Y) % mapSize.Y;
        if (r.Pos.Y < 0) r.Pos.Y += mapSize.Y;
        map[r.Pos.Y][r.Pos.X]++;
    }
}

static void VisualizeMap(long iterNum, Dictionary<int, Dictionary<int, int>> map, Coordinate mapSize)
{
    StringBuilder sb = new StringBuilder();
    sb.AppendLine($"Iter: {iterNum}");

    for (int y = 0; y < mapSize.Y; y++)
        sb.AppendLine(string.Join("", map[y].Select(k => k.Value == 0 ? "." : "#")));

    File.AppendAllText("out.txt", sb.ToString());
}

    var input = File.ReadAllLines("input.txt").ToList();
long ret = 0;

Coordinate mapSize = new Coordinate { X = 101, Y = 103 };

List<Robot> robots = new List<Robot>();
Dictionary<int, Dictionary<int, int>> map = new Dictionary<int, Dictionary<int, int>>();
for (int y = 0; y < mapSize.Y; y++)
{
    map.Add(y, new Dictionary<int, int>());
    for (int x = 0; x < mapSize.X; x++)
    {
        map[y].Add(x, 0);
    }
}

foreach (var line in input)
{
    var ls = line.Split(" ");
    var ps = ls[0].Replace("p=", "").Split(',');
    var vs = ls[1].Replace("v=", "").Split(",");

    var r = new Robot { Pos = new Coordinate { X = int.Parse(ps[0]), Y = int.Parse(ps[1]) }, V = new Coordinate { X = int.Parse(vs[0]), Y = int.Parse(vs[1]) } };
    robots.Add(r);
    map[r.Pos.Y][r.Pos.X]++;
}
int i = 0;
while (true)
{
    i++;
    foreach (var r in robots)
    {
        SimulateRobot(r, mapSize, 1, map);
    }
    VisualizeMap(i, map, mapSize);
}
var r1 = robots.Where(r => r.Pos.X < mapSize.X / 2 && r.Pos.Y < mapSize.Y / 2).Count();
var r2 = robots.Where(r => r.Pos.X > mapSize.X / 2 && r.Pos.Y < mapSize.Y / 2).Count();
var r3 = robots.Where(r => r.Pos.X < mapSize.X / 2 && r.Pos.Y > mapSize.Y / 2).Count();
var r4 = robots.Where(r => r.Pos.X > mapSize.X / 2 && r.Pos.Y > mapSize.Y / 2).Count();

ret = r1 * r2 * r3 * r4;

Console.WriteLine(ret);
Console.ReadLine();

public class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class Robot
{
    public Coordinate Pos { get; set; }
    public Coordinate V { get; set; }
}