static bool InOrder(Dictionary<int, HashSet<int>> rules, List<int> update)
{
    for (int i = 0; i < update.Count; i++)
    {
        var un = update[i];
        for (int j = i + 1; j < update.Count; j++)
        {
            var nn = update[j];
            if (rules.ContainsKey(nn) && rules[nn].Contains(un))
            {
                return false;
            }
        }
    }
    return true;
}

static int Middle(List<int> update)
{
    return update[update.Count / 2];
}


static List<int> Order(Dictionary<int, HashSet<int>> rules, List<int> update)
{
    List<int> ret = new List<int>();
    var rr = update.ToList();
    while(ret.Count != update.Count)
    {
        foreach(var u in rr)
        {
            if(!rules.ContainsKey(u) || !rules[u].Intersect(rr).Any())
            {
                rr.Remove(u);
                ret.Insert(0, u);
                break;
            }
        }
    }
    return ret;
}

var input = File.ReadAllLines("input.txt");

var ret1 = 0;
var ret2 = 0;
Dictionary<int, HashSet<int>> rules = new Dictionary<int, HashSet<int>>();
int i = 0;
for(i = 0; i < input.Length; i++)
{
    var line = input[i];
    if (line == "")
        break;
    var ss = line.Split('|');
    var p = int.Parse(ss[0]);
    if (!rules.ContainsKey(p))
    {
        rules.Add(p, new HashSet<int>());
    }
    rules[p].Add(int.Parse(ss[1]));
}

for(i = i + 1; i < input.Length; i++)
{
    var updatestr = input[i];
    var update = updatestr.Split(',').Select(s => int.Parse(s)).ToList();

    if(InOrder(rules, update ))
    {
        ret1 += Middle(update);
    }
    else
    {
        var nu = Order(rules, update);
        ret2 += Middle(nu);
    }
}

Console.WriteLine(ret1);
Console.WriteLine(ret2);
Console.ReadLine();


class Rule
{
    public int Page { get; set; }
    public int PageAfter { get; set; }
}

