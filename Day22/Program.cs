long Next(long num)
{
    var ret = num * 64;
    ret = ret ^ num;
    ret = ret % 16777216;

    var r2 = ret / 32;
    ret = r2 ^ ret;
    ret = ret % 16777216;

    var r3 = ret * 2048;
    ret = r3 ^ ret;
    ret = ret % 16777216;

    return ret;
}

var input = File.ReadAllLines("input.txt").ToList();
long ret = 0;

Dictionary<string, long> ret2 = new Dictionary<string, long>();
HashSet<string> ret2cache = new HashSet<string>();

foreach(var line in input)
{
    ret2cache.Clear();
    var n = long.Parse(line);
    string key = "0";
    int lp = 0;
    for(int i = 0;i < 2000; i++)
    {
        var p = int.Parse(n.ToString().Last().ToString());
        if (i > 0)
        {
            var diff = p - lp;
            if (i < 4)
            {
                key = key + diff.ToString();
            }
            else
            {
                if (key.First() == '-')
                    key = key.Substring(2) + diff.ToString();
                else
                    key = key.Substring(1) + diff.ToString();
            }
        }

        if(i > 3 && !ret2cache.Contains(key))
        {
            ret2cache.Add(key);
            if (!ret2.ContainsKey(key))
                ret2[key] = 0;
            ret2[key] += p;
        }

        n = Next(n);
        lp = p;
    }
    ret += n;
}

var ret2v = ret2.Values.OrderByDescending(v=>v).First();

Console.WriteLine(ret);
Console.WriteLine(ret2v);
Console.ReadLine();