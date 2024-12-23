long GetPartLength(int iter, string part, int maxIter, DirPad dp, Dictionary<int, Dictionary<string, long>> cache)
{
    if (!cache.ContainsKey(iter))
        cache.Add(iter, new Dictionary<string, long>());

    if (!cache.ContainsKey(iter+1))
        cache.Add(iter+1, new Dictionary<string, long>());


    if (cache[iter].ContainsKey(part))
        return cache[iter][part];

    if (iter == maxIter)
    {
        cache[iter].Add(part, part.Length);
        return part.Length;
    }

    var trets = new List<string> { "" };
    for (int i = 0; i < part.Length; i++)
    {
        var s = dp.FromTo(i < 1 ? DirPad.nums['A'] : DirPad.nums[part[i - 1]], DirPad.nums[part[i]]);
        trets = trets.SelectMany(fl => s.Select(sl => fl + sl + "A")).Distinct().ToList();
    }

    var mintl = trets.Min(t => t.Length);
    var ts = trets.Where(t => t.Length == mintl);
    var newParts = ts.SelectMany(t => t.Split("A").Select(s => s + "A")).Distinct().ToList();
    if(iter==0)
    {
        if (ts.Select(t => t.Split("A").Select(s => s + "A")).Any(s => s.Any(ss => ss == "<A")))
        {

        }
    }
    foreach(var np in newParts)
    {
        if (!cache[iter + 1].ContainsKey(np))
        {
            var npr = GetPartLength(iter + 1, np, maxIter, dp, cache);
        }
    }
    
    var tss = ts.Select(t => t.Split("A").Select(s => s + "A")).Select(t => t.Sum(tt => cache[iter + 1][tt]));
    var ret = tss.Min(t => t) - 1;
    cache[iter].Add(part, ret);
    if (iter == 1)
    {
       
    }
    return ret;
}


NumPad np = new NumPad();
DirPad dp = new DirPad();

var ret1 = 0;
long ret2 = 0;
var input = File.ReadAllLines("input.txt").ToList();
var cache = new Dictionary<int, Dictionary<string, long>>();
var maxIter = 25;

foreach (var line in input)
{
    var cs = np.CodeCoordinates(line);

    List<string> numPadSeq = new List<string> { "" };
    for (int i = 0; i < cs.Count; i++)
    {
        var s = np.FromTo(i < 1 ? NumPad.nums['A'] : cs[i - 1], cs[i]);
        List<string> rets = new List<string>();
        foreach (var r in numPadSeq)
            foreach (var ss in s)
            {
                rets.Add(r + ss + "A");
            }
        numPadSeq = rets;
    }

    long pr22 = long.MaxValue;
    foreach(var nps in numPadSeq)
    {
        var ps = nps.Split("A").Select(s => s + "A").ToList();
        long pr2 = 0;
        foreach(var p in ps)
        {
            pr2 += GetPartLength(0, p, maxIter, dp, cache);
        }
        
        if (pr22 > pr2 - 1)
            pr22 = pr2 - 1;
    }
    var num = int.Parse(line.Substring(0, line.Length - 1));
    Console.WriteLine($"{pr22} * {num}");
    ret2 += num * pr22;

}

//foreach (var line in input)
//{
//    var cs = np.CodeCoordinates(line);

//    List<string> numPadSeq = new List<string> { "" };
//    for (int i = 0; i < cs.Count; i++)
//    {
//        var s = np.FromTo(i < 1 ? NumPad.nums['A'] : cs[i - 1], cs[i]);
//        List<string> rets = new List<string>();
//        foreach (var r in numPadSeq)
//            foreach (var ss in s)
//            {
//                rets.Add(r + ss + "A");
//            }
//        numPadSeq = rets;
//    }

//    List<string> dirSeq = numPadSeq.ToList();
//    List<string> retSeq = new List<string>();
//    List<string> currList = null;
//    foreach (var dsr in dirSeq)
//    {
//        for(int rs = 0; rs < maxIter; rs++)
//        {
//            if (!cache.ContainsKey(rs))
//                cache.Add(rs, new Dictionary<string, List<string>>());

//            if (rs == 0)
//                currList = new List<string> { dsr };


//            var tempList = new List<string>();
//            foreach (var rr in currList)
//            {
//                if (!cache[rs].ContainsKey(rr))
//                {
//                    var rrparts = rr.Split("A").Select(s => s + "A").ToList();
//                    List<string> trets = new List<string> { "" };
//                    foreach (var part in rrparts)
//                    {
//                        for (int i = 0; i < part.Length; i++)
//                        {
//                            var s = dp.FromTo(i < 1 ? DirPad.nums['A'] : DirPad.nums[part[i - 1]], DirPad.nums[part[i]]);
//                            trets = trets.SelectMany(fl => s.Select(sl => fl + sl + "A")).Distinct().ToList();
//                        }
//                    }
//                    var ttl = trets.Select(t => t.Substring(0, t.Length - 1)).ToList();
//                    cache[rs][rr] = ttl;
//                    tempList.AddRange(ttl);
//                }
//                else
//                {
//                    tempList.AddRange(cache[rs][rr]);
//                }
//            }
//            currList = tempList;
//            if (rs == maxIter - 1)
//                retSeq.AddRange(currList);
//        }
//    }

//    var num = int.Parse(line.Substring(0, line.Length - 1));
//    var x = retSeq.OrderBy(c => c.Length).First().Length;
//    Console.WriteLine($"{x} * {num}");

//    ret2 += x * num;


//for (int rs = 0; rs < 2; rs++)
//{
//    if (!cache.ContainsKey(rs))
//        cache.Add(rs, new Dictionary<string, List<string>>());

//    retSeq.Clear();
//    foreach (var rr in dirSeq)
//    {
//        var rrparts = rr.Split("A").Select(s=>s+"A").ToList();
//        List<string> trets = new List<string> { "" };

//        foreach (var part in rrparts)
//        {
//            for (int i = 0; i < part.Length; i++)
//            {
//                var s = dp.FromTo(i < 1 ? DirPad.nums['A'] : DirPad.nums[part[i - 1]], DirPad.nums[part[i]]);
//                trets = trets.SelectMany(fl => s.Select(sl => fl + sl + "A")).Distinct().ToList();
//            }
//        }

//        retSeq.AddRange(trets.Select(t=>t.Substring(0, t.Length - 1)));
//    }
//    dirSeq = retSeq.ToList();
//}

//var x = retSeq.Select(r => r.Length).OrderBy(v => v).First();
//var num = int.Parse(line.Substring(0, line.Length - 1));
//ret1 += x * num;
//Console.WriteLine($"{x} * {num}");
//}
Console.WriteLine(ret2);
Console.ReadLine();

class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
}

class NumPad
{
    public static Dictionary<char, Coordinate> nums = new Dictionary<char, Coordinate>
    {
        { '7', new Coordinate{Y = 0, X = 0 } },
        { '8', new Coordinate{Y = 0, X = 1 } },
        { '9', new Coordinate{Y = 0, X = 2 } },
        { '4', new Coordinate{Y = 1, X = 0 } },
        { '5', new Coordinate{Y = 1, X = 1 } },
        { '6', new Coordinate{Y = 1, X = 2 } },
        { '1', new Coordinate{Y = 2, X = 0 } },
        { '2', new Coordinate{Y = 2, X = 1 } },
        { '3', new Coordinate{Y = 2, X = 2 } },
        { '0', new Coordinate{Y = 3, X = 1 } },
        { 'A', new Coordinate{Y = 3, X = 2 } },
    };

    public List<Coordinate> CodeCoordinates(string code)
    {
        var ret = new List<Coordinate>();
        foreach (var c in code)
        {
            ret.Add(nums[c]);
        }
        return ret;
    }

    public List<string> FromTo(Coordinate from, Coordinate to)
    {
        var dy = from.Y - to.Y;
        var dx = from.X - to.X;
        var l = new List<string>
        {
            (dy < 0 ? new string('v', -1 * dy) : new string('^', dy)) + (dx < 0 ? new string('>', -1 * dx) : new string('<', dx)),
            (dx < 0 ? new string('>', -1 * dx) : new string('<', dx)) + (dy < 0 ? new string('v', -1 * dy) : new string('^', dy)),
        };
        
        var ret = new List<string>();
        foreach (var r in l)
        {
            var t = new Coordinate {X = from.X, Y = from.Y };
            bool good = true;
            foreach (var c in r)
            {
                switch (c)
                {
                    case '<': t.X--; break;
                    case '>': t.X++; break;
                    case '^': t.Y--; break;
                    case 'v': t.Y++; break;
                }
                if(t.Y == 3 && t.X == 0)
                {
                    good = false;
                    break;
                }
            }
            if (good)
                ret.Add(r);
        }
        return ret.Distinct().OrderBy(v=>v.Length).Take(2).ToList();
    }

}

class DirPad
{
    public static Dictionary<char, Coordinate> nums = new Dictionary<char, Coordinate>
    {
        { '<', new Coordinate{Y = 1, X = 0 } },
        { 'v', new Coordinate{Y = 1, X = 1 } },
        { '>', new Coordinate{Y = 1, X = 2 } },
        { '^', new Coordinate{Y = 0, X = 1 } },
        { 'A', new Coordinate{Y = 0, X = 2 } },
    };

    public List<Coordinate> CodeCoordinates(string code)
    {
        var ret = new List<Coordinate>();
        foreach (var c in code)
        {
            ret.Add(nums[c]);
        }
        return ret;
    }

    public List<string> FromTo(Coordinate from, Coordinate to)
    {
        var dy = from.Y - to.Y;
        var dx = from.X - to.X;
        var l = new List<string>
        {
            (dy < 0 ? new string('v', -1 * dy) : new string('^', dy)) + (dx < 0 ? new string('>', -1 * dx) : new string('<', dx)),
            (dx < 0 ? new string('>', -1 * dx) : new string('<', dx)) + (dy < 0 ? new string('v', -1 * dy) : new string('^', dy)),
        };

        var ret = new List<string>();
        foreach (var r in l)
        {
            var t = new Coordinate { X = from.X, Y = from.Y };
            bool good = true;
            foreach (var c in r)
            {
                switch (c)
                {
                    case '<': t.X--; break;
                    case '>': t.X++; break;
                    case '^': t.Y--; break;
                    case 'v': t.Y++; break;
                }
                if (t.Y == 0 && t.X == 0)
                {
                    good = false;
                    break;
                }
            }
            if (good)
                ret.Add(r);
        }
        return ret.Distinct().ToList();
    }

}