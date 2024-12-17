using System.Text;

var input = File.ReadAllLines("input.txt").ToList();
long ret = 0;

IntComp comp = new IntComp();
foreach (var line in input)
{
    if (line.StartsWith("Register A: "))
        comp.A = long.Parse(line.Replace("Register A: ", ""));
    if (line.StartsWith("Register B: "))
        comp.B = long.Parse(line.Replace("Register B: ", ""));
    if (line.StartsWith("Register C: "))
        comp.C = long.Parse(line.Replace("Register C: ", ""));

    if (line.StartsWith("Program: "))
        comp.Instructions = line.Replace("Program: ", "").Split(",").Select(s => long.Parse(s)).ToList();

}

long i = 0;
var rr = string.Join(",", comp.Instructions);

while (comp.DoOp() != true)
{
}

string ret1 = string.Join(",", comp.Output);
Console.WriteLine(ret1);

long ret2 = 0;
string goal = "2415751603465530";
StringBuilder sb = new StringBuilder();
i = (long)Math.Pow(8, 15);
var e = 14;
while (true)
{
    long A = i;
    long B = 0;
    long C = 0;

    sb.Clear();
    while (true)
    {
        B = A % 8;
        B = B ^ 5;
        C = A / (long)Math.Pow(2, B);
        B = B ^ 6;
        A = A / 8;
        B = B ^ C;
        sb.Append(B % 8);
        if (A == 0)
            break;
    }
                         
    if (sb.ToString() == goal)
    {
        ret2 = i;
        break;
    }
    else
    {
        if(sb.Length > 16)
        {
            return;
        }
        if (sb[e + 1] == goal[e+1])
        {
            if (sb.ToString().Substring(e + 1) != goal.Substring(e + 1))
            {
                e++;
            }
            else
            {
                e--;
            }
            if (e == -2)
                break;
        }
        i += (long)Math.Pow(8, e < 0 ? 0 : e);
    }
}

Console.WriteLine(ret2);
Console.ReadLine();

class IntComp
{
    public long A { get; set; }
    public long B { get; set; }
    public long C { get; set; }

    public int Pointer { get; set; }
    public List<long> Instructions { get; set; }

    public List<long> Output { get; set; } = new List<long>();

    private long OperandValue(long value)
    {
        if (value < 4)
            return value;
        if (value == 4)
            return A;
        if (value == 5)
            return B;
        if (value == 6)
            return C;
        throw new Exception();
    }

    public bool DoOp()
    {
        if (Pointer >= Instructions.Count)
            return true;

        var opCode = Instructions[Pointer];
        var operand = Instructions[Pointer + 1];
        long ov;

        bool jumped = false;
        switch (opCode)
        {
            case 0:
                ov = OperandValue(operand);
                A = A / (long)Math.Pow(2, ov); 
                break;
            case 1:
                B = B ^ operand;
                break;
            case 2:
                ov = OperandValue(operand);
                B = ov % 8;
                break;
            case 3:
                if(A != 0)
                {
                    Pointer = (int)operand;
                    jumped = true;
                }
                break;
            case 4:
                B = B ^ C;
                break;
            case 5:
                ov = OperandValue(operand);
                Output.Add(ov % 8);
                //Console.Write($"{ov % 8},");
                break;
            case 6:
                ov = OperandValue(operand);
                B = A / (long)Math.Pow(2, ov);
                break;
            case 7:
                ov = OperandValue(operand);
                C = A / (long)Math.Pow(2, ov);
                break;
        }

        if (!jumped)
            Pointer += 2;
        return false;
    }
}