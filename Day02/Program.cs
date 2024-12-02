var input = File.ReadAllLines("input.txt");

var ls = input.Select(s => s.Split(" ").Select(i => int.Parse(i)).ToList()).ToList();
var ret2 = 0;
foreach (var l in ls)
{
    for (int d = 0; d < l.Count; d++)
    {
        var lm = l.Take(d).Concat(l.Skip(d+1)).ToList();

        int last = lm[0];
        bool asc = lm[1] > lm[0];
        bool safe = true;
        foreach (var i in lm.Skip(1))
        {
            if (asc && i < last)
            {
                safe = false;
                break;
            }
            else if (!asc && i > last)
            {
                safe = false;
                break;
            }
            if (Math.Abs(last - i) < 1 || Math.Abs(last - i) > 3)
            {
                safe = false;
                break;
            }
            last = i;
        }

        if (safe)
        {
            ret2++;
            break;
        }
    }
}

Console.ReadLine();
