(bool, List<int>) Parse(List<string> curr)
{
    bool isLock = curr.First().First() == '#';
    List<int> clok = new List<int> { -1, -1, -1, -1, -1 };
    if (isLock)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (clok[j] == -1 && curr[i + 1][j] == '.')
                {
                    clok[j] = i;
                }
            }
        }
        return (true, clok);
    }
    else
    {
        for (int i = 6; i > 0; i--)
        {
            for (int j = 0; j < 5; j++)
            {
                if (clok[j] == -1 && curr[i - 1][j] == '.')
                {
                    clok[j] = 6 - i;
                }
            }
        }
        return (false, clok);
    }

}


var input = File.ReadAllLines("input.txt").ToList();
List<List<int>> keys = new List<List<int>>();
List<List<int>> locks = new List<List<int>>();

List<string> curr = new List<string>();
bool islock;
List<int> pv;

foreach (var line in input)
{
    if(line == "")
    {
        (islock, pv) = Parse(curr);
        if (islock)
            locks.Add(pv);
        else
            keys.Add(pv);
        curr.Clear();
    }
    else
    {
        curr.Add(line);
    }
}

(islock, pv) = Parse(curr);
if (islock)
    locks.Add(pv);
else
    keys.Add(pv);

var ret1 = 0;
foreach(var k in keys)
{
    foreach(var l in locks)
    {
        if (k.Zip(l).All(t => t.First + t.Second <= 5))
            ret1++;
    }
}

Console.WriteLine(ret1);
Console.ReadLine();