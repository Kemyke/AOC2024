using System.Text.RegularExpressions;

var input = File.ReadAllText("input.txt");
Regex r = new Regex("mul\\((\\d+),(\\d+)\\)|do\\(\\)|don't\\(\\)");

long ret1 = 0;
long ret2 = 0;
bool domul = true;
var ms = r.Matches(input);
foreach(Match m in ms)
{
    if (m.Value == "do()")
    {
        domul = true;
    }
    else if (m.Value == "don't()")
    {
        domul = false;
    }
    else
    {
        var a = int.Parse(m.Groups[1].Value);
        var b = int.Parse(m.Groups[2].Value);

        ret1 += a * b;

        if (domul)
        {
            ret2 += a * b;
        }
    }
}

Console.ReadLine();