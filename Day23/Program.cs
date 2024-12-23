var input = File.ReadAllLines("input.txt").ToList();
Dictionary<string, Computer> comps = new Dictionary<string, Computer>();

foreach(var line in input)
{
    var ss = line.Split("-");
    if (!comps.ContainsKey(ss[0]))
        comps.Add(ss[0], new Computer { Name = ss[0] });
    if (!comps.ContainsKey(ss[1]))
        comps.Add(ss[1], new Computer { Name = ss[1] });

    comps[ss[0]].Connection.Add(ss[1]);
    comps[ss[1]].Connection.Add(ss[0]);
}

HashSet<string> triplets = new HashSet<string>();

foreach(var cn in comps.Keys)
{
    var comp = comps[cn];

    foreach(var c1 in comp.Connection)
    {
        foreach(var c2 in comp.Connection)
        {
            if (c1 != c2 && comps[c1].Connection.Contains(c2))
            {
                triplets.Add(string.Join("", new List<string> { cn, c1, c2 }.OrderBy(c => c)));
            }
        }
    }
}

var ret1 = triplets.Count(t => t[0] == 't' || t[2] == 't' || t[4] == 't');
string rr2 = "";

while (triplets.Any())
{
    var ol = triplets.ToList();
    triplets.Clear();
    foreach (var t in ol)
    {
        var ncs = t.Chunk(2).Select(c => new string(c)).ToHashSet();
        foreach (var c in comps)
        {
            if(!ncs.Contains(c.Key) && ncs.All(nc => comps[nc].Connection.Contains(c.Key)))
            {
                triplets.Add(string.Join("", ncs.Append(c.Key).OrderBy(c => c)));
            }
        }
    }
    rr2 = ol.First();
}

Console.WriteLine(ret1);
Console.WriteLine(string.Join(",",rr2.Chunk(2).Select(c => new string(c))));
Console.ReadLine();

class Computer
{
    public string Name { get; set; }
    public HashSet<string> Connection { get; set; } = new HashSet<string>();
}