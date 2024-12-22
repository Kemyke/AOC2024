NumPad np = new NumPad();
DirPad dp = new DirPad();

var ret1 = 0;

var input = File.ReadAllLines("input.txt").ToList();
foreach (var line in input)
{
    var cs = np.CodeCoordinates(line);
    //List<string> rett = new List<string> ();
    //List<string> rettt = new List<string> ();
    
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

    List<string> dirSeq = numPadSeq.ToList();
    List<string> retSeq = new List<string>();
    for(int rs = 0; rs < 2; rs++)
    {
        retSeq.Clear();
        foreach (var rr in dirSeq)
        {

            List<string> trets = new List<string> { "" };
            for (int i = 0; i < rr.Length; i++)
            {
                var s = dp.FromTo(i < 1 ? DirPad.nums['A'] : DirPad.nums[rr[i - 1]], DirPad.nums[rr[i]]);
                List<string> rets = new List<string>();
                foreach (var r in trets)
                    foreach (var ss in s)
                    {
                        rets.Add(r + ss + "A");
                    }
                trets = rets;
            }
            retSeq.AddRange(trets);
        }
        dirSeq = retSeq.ToList();
    }

    var x = retSeq.Select(r => r.Length).OrderBy(v => v).First();
    var num = int.Parse(line.Substring(0, line.Length - 1));
    ret1 += x * num;
    Console.WriteLine($"{x} * {num}");
}
Console.WriteLine(ret1);
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