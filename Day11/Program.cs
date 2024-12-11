
static void FillCache(Dictionary<long, List<long>> cache, long os)
{
    var oldList = new List<long> { os };
    for (int i = 0; i < 25; i++)
    {
        var newList = new List<long>();
        foreach (var stone in oldList)
        {
            if (stone == 0)
                newList.Add(1);
            else
            {
                var ss = stone.ToString();
                if (ss.Length % 2 == 0)
                {
                    newList.Add(long.Parse(ss.Substring(0, ss.Length / 2)));
                    newList.Add(long.Parse(ss.Substring(ss.Length / 2)));
                }
                else
                {
                    newList.Add(stone * 2024);
                }
            }
        }
        oldList = newList;
    }
    cache.Add(os, oldList);
}

var input = File.ReadAllText("input.txt").Split(" ").Select(s=>long.Parse(s)).ToList();
var ret = 0;

var cache = new Dictionary<long, List<long>>();

var res = new List<long>();
var res2 = new Dictionary<long, long> { };
foreach (var os in input)
{
    if (!cache.ContainsKey(os))
        FillCache(cache, os);

    foreach (var os2 in cache[os])
    {
        if (!res2.ContainsKey(os2))
            res2.Add(os2, 0);

        res2[os2]++;
    }
}
Console.WriteLine($"25: {res2.Values.Sum()}");
var ol = res.ToList();
var res3 = new Dictionary<long, long> { };
foreach (var os in res2)
{
    if (!cache.ContainsKey(os.Key))
        FillCache(cache, os.Key);

    foreach (var os2 in cache[os.Key])
    {
        if (!res3.ContainsKey(os2))
            res3.Add(os2, 0);

        res3[os2]+=os.Value;
    }
}

Console.WriteLine($"50: {res3.Values.Sum()}");

var res4 = new Dictionary<long, long> { };
foreach (var os in res3)
{
    if (!cache.ContainsKey(os.Key))
        FillCache(cache, os.Key);

    foreach (var os2 in cache[os.Key])
    {
        if (!res4.ContainsKey(os2))
            res4.Add(os2, 0);

        res4[os2] += os.Value;
    }
}
Console.WriteLine($"75: {res4.Values.Sum()}");

Console.ReadLine();
