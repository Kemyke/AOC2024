static bool Possible(string pattern, HashSet<string> posPatterns, Dictionary<string, bool> cache)
{
    if(cache.ContainsKey(pattern))
        return cache[pattern];

    if (posPatterns.Contains(pattern))
    {
        cache.Add(pattern, true);
        return true;
    }
    var pp = posPatterns.Where(p => pattern.StartsWith(p));
    var ret =  pp.Any(p => Possible(pattern.Substring(p.Length), posPatterns, cache));
    cache.Add(pattern, ret);

    return ret;
}

static decimal Possible2(string pattern, HashSet<string> posPatterns, Dictionary<string, decimal> cache)
{
    if (cache.ContainsKey(pattern))
        return cache[pattern];
    decimal ret = 0;

    if (posPatterns.Contains(pattern))
    {
        ret++;
    }

    var pp = posPatterns.Where(p => pattern.StartsWith(p) && pattern.Length > p.Length);
    foreach (var ppp in pp)
    {
        ret += Possible2(pattern.Substring(ppp.Length), posPatterns, cache);
    }
    cache.Add(pattern, ret);

    return ret;
}

Dictionary<string, bool> cache = new Dictionary<string, bool>();
Dictionary<string, decimal> cache2 = new Dictionary<string, decimal>();

var input = File.ReadAllLines("input.txt").ToList();
var ret = 0;
decimal ret2 = 0;

var posPatterns = input.First().Split(", ").ToHashSet();

foreach (var line in input.Skip(2))
{
    var pr = Possible2(line, posPatterns, cache2);
    if (pr > 0)
        ret++;
    ret2 += pr;
}

Console.WriteLine(ret);
Console.WriteLine(ret2);
Console.ReadLine();