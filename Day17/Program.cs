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
        comp.Instructions = line.Replace("Program: ", "").Split(",").Select(s=>long.Parse(s)).ToList();

}

long i = 674376090;
var rr = string.Join(",", comp.Instructions);
while (true)
{
    var n = new IntComp { B = comp.B, C = comp.C, Instructions = comp.Instructions };
    n.A = i++;
    Console.WriteLine($"A: {n.A} B: {n.B} C: {n.C}");

    while (n.DoOp() != true)
    {
        Console.WriteLine($"A: {n.A} B: {n.B} C: {n.C}");

        var pr = string.Join(",", n.Output);
        if (!rr.StartsWith(pr))
        {
            if (pr.Length > 14)
                Console.WriteLine($"{i - 1} {pr}");

            break;
        }
    }

    var r = string.Join(",", n.Output);
    if (r == rr)
        break;
}

Console.WriteLine(i - 1);
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