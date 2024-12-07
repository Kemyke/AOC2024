var input = File.ReadAllLines("input.txt");
long ret = 0;

foreach (var line in input)
{
    var ss = line.Split(':');
    var res = long.Parse(ss[0]);
    var values = ss[1].Trim().Split(' ').Select(v=>long.Parse(v)).ToList();
    var eqs = new HashSet<long> { values.First() };
    foreach(var v in values.Skip(1))
    {
        eqs = eqs.SelectMany(e => new List<long> { e * v, e + v, long.Parse(e.ToString() + v.ToString()) }).ToHashSet();
    }
    if(eqs.Any(e=>e==res))
    {
        ret += res;
    }
}

Console.WriteLine(ret);
Console.ReadLine();