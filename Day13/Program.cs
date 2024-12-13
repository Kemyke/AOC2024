static long? SolveMachine(Machine m)
{
    if ((m.BdY * m.PX - m.BdX * m.PY) % (decimal)(m.AdX * m.BdY - m.BdX * m.AdY) == 0 &&
        (m.AdX * m.PY - m.AdY * m.PX) % (decimal)(m.AdX * m.BdY - m.BdX * m.AdY) == 0)
    {
        var i = (m.BdY * m.PX - m.BdX * m.PY) / (m.AdX * m.BdY - m.BdX * m.AdY);
        var j = (m.AdX * m.PY - m.AdY * m.PX) / (m.AdX * m.BdY - m.BdX * m.AdY);
        return i * 3 + j;
    }
    return 0;
}

var input = File.ReadAllLines("input.txt").ToList();
long ret = 0;

var machines = new List<Machine>();

var m = new Machine();
foreach (var line in input)
{
    if (line == "")
    {
        machines.Add(m);
        m = new Machine();
    }
    else if(line.StartsWith("Button A:"))
    {
        var ss = line.Replace("Button A: ", "").Split(", ").SelectMany(s => s.Split("+")).ToList();
        m.AdX = int.Parse(ss[1]);
        m.AdY = int.Parse(ss[3]);
    }
    else if (line.StartsWith("Button B:"))
    {
        var ss = line.Replace("Button B: ", "").Split(", ").SelectMany(s => s.Split("+")).ToList();
        m.BdX = int.Parse(ss[1]);
        m.BdY = int.Parse(ss[3]);
    }
    else if (line.StartsWith("Prize:"))
    {
        var ss = line.Replace("Prize: ", "").Split(", ").SelectMany(s => s.Split("=")).ToList();
        m.PX = int.Parse(ss[1]);
        m.PY = int.Parse(ss[3]);
        //part2
        m.PX += 10000000000000;
        m.PY += 10000000000000;
    }
}
machines.Add(m);

foreach(var machine in machines)
{
    var sm = SolveMachine(machine);
    if (sm != null)
        ret += sm.Value;
}

Console.WriteLine(ret);
Console.ReadLine();

class Machine
{
    public long AdX { get; set; }
    public long AdY { get; set; }
    public long BdX { get; set; }
    public long BdY { get; set; }
           
    public long PX { get; set; }
    public long PY { get; set; }
}