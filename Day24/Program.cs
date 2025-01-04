using System.Text;

List<string> GetConnected(string gate, Dictionary<string, Gate> gates)
{
    List<string> res = new List<string>();
    var i1 = gates[gate].Input1;
    var i2 = gates[gate].Input2;
    if(i1 != null)
    {
        res.Add(i1);
        res.AddRange(GetConnected(i1, gates));
    }
    if (i2 != null)
    {
        res.Add(i2);
        res.AddRange(GetConnected(i2, gates));
    }
    return res;
}

string GetConnected2(string gate, Dictionary<string, Gate> gates)
{
    string ret;
    var i1 = gates[gate].Input1;
    string il1;
    var i2 = gates[gate].Input2;
    string il2;
    if (i1 == null && i2 == null)
    {
        ret = gate;
    }
    else
    {
        il1 = GetConnected2(i1, gates);
        il2 = GetConnected2(i2, gates);
        ret = $"{il2} {gates[gate].GateType} {il1}";
    }
    return ret;
}


long Compute(Dictionary<string, Gate> gates, List<string> resultGates)
{
    while (!resultGates.All(g => gates[g].Result.HasValue))
    {
        foreach (var kvp in gates.Where(g => !g.Value.Result.HasValue))
        {
            kvp.Value.GetResult(gates, 0);
            //Console.WriteLine(kvp.Key);
        }
    }
    var bs = string.Join("", resultGates.Select(g => gates[g].Result.Value ? "1" : "0"));
    var ret = Convert.ToInt64(bs, 2);
    return ret;
}

void Clear(Dictionary<string, Gate> gates)
{
    foreach(var g in gates.Values)
    {
        g.Result = null;
        g.SwappedName = null;
    }    
}

var input = File.ReadAllLines("input.txt").ToList();
long ret1 = 0;

Dictionary<string, Gate> gates = new Dictionary<string, Gate>();
int lc = 0;
foreach(var line in input)
{
    if (line == "")
        break;
    var ss = line.Split(": ");
    gates.Add(ss[0], new Gate { Name = ss[0], InitResult = int.Parse(ss[1]) == 1 ? true : false });
    lc++;
}

foreach(var line in input.Skip(lc + 1))
{
    var s1 = line.Split(" -> ");
    var s2 = s1[0].Split(" ");
    gates.Add(s1[1], new Gate { Name = s1[1], GateType = s2[1], Input1 = s2[0], Input2 = s2[2] });
}

var input1Gates = gates.Keys.Where(g => g.StartsWith("x")).OrderByDescending(g => g);
var input1Bin = string.Join("", input1Gates.Select(g => gates[g].InitResult ? "1" : "0"));
var input1Num = Convert.ToInt64(input1Bin, 2);

var input2Gates = gates.Keys.Where(g => g.StartsWith("y")).OrderByDescending(g => g);
var input2Bin = string.Join("", input2Gates.Select(g => gates[g].InitResult ? "1" : "0"));
var input2Num = Convert.ToInt64(input2Bin, 2);


var resultGates = gates.Keys.Where(g => g.StartsWith("z")).OrderByDescending(g=>g).ToList();
ret1 = Compute(gates, resultGates);
Console.WriteLine(ret1);
Console.WriteLine(Convert.ToString(ret1,2));

var fixSwaps1 = new List<Gate>();
var fixSwaps2 = new List<Gate>();
foreach(var gate in resultGates)
{
    if (gates[gate].GateType != "XOR" && gate != "z45")
        fixSwaps1.Add(gates[gate]);

    //var r = GetConnected2(gate, gates);
    //Console.WriteLine($"{gate}: {r}");
}

foreach(var gate in gates.Keys.Except(input1Gates).Except(input2Gates).Except(resultGates))
{
    var g = gates[gate];
    if (g.Input1[0] != 'y' && g.Input1[0] != 'x' && g.Input2[0] != 'y' && g.Input2[0] != 'x' && g.GateType == "XOR")
        fixSwaps2.Add(gates[gate]);
}

var fxsws = fixSwaps1.Select(s => fixSwaps2.Select(s2 => new List<(string, string)> { (s.Name, s2.Name) }).ToList()).ToList();


var sbits = new List<string>();
var nsbits = new List<string>();
for (int i = 2; i < 16; i++)
{
    sbits.AddRange(GetConnected(resultGates[i], gates));
}

for (int i = 16; i < 46; i++)
{
    nsbits.AddRange(GetConnected(gates[resultGates[i]].Input1, gates));
    nsbits.AddRange(GetConnected(gates[resultGates[i]].Input2, gates));
}
sbits = sbits.Distinct().Except(input1Gates).Except(input2Gates).ToList();
nsbits = nsbits.Distinct().Except(input1Gates).Except(input2Gates).ToList();
var psbits = sbits.Except(nsbits).Except(fixSwaps1.Select(fs=>fs.Name)).Except(fixSwaps2.Select(fs => fs.Name)).ToList();
//Console.WriteLine(string.Join(",", psbits));

var npbits = gates.Keys.Except(input1Gates).Except(input2Gates).Except(fixSwaps1.Select(fs => fs.Name)).Except(fixSwaps2.Select(fs => fs.Name)).ToList();

List<((string, string), (string, string), (string, string), (string, string))> swaps = new List<((string, string), (string, string), (string, string), (string, string))>();

var expRes = input1Num + input2Num;

Console.WriteLine();
Console.WriteLine(expRes);
Console.WriteLine(Convert.ToString(expRes, 2));
//new List<(string, string)> { ("wdg", "wdg") }) 
foreach (var sw1 in new List<(string, string)> { ("wdg", "wdg") })  //npbits.SelectMany((x, i) => npbits.Skip(i + 1), (x, y) => (x, y)))
{
    foreach (var sw2 in fxsws[0])
    {
        foreach (var sw3 in fxsws[1])
        {
            foreach (var sw4 in fxsws[2])
            {
                var ssw2 = sw2.Single();
                var ssw3 = sw3.Single();
                var ssw4 = sw4.Single();

                //z32,gfm,z16,mrb,z08,cdj

                //if (ssw2.Item2 != ssw3.Item2 && ssw3.Item2 != ssw4.Item2 && ssw2.Item2 != ssw4.Item2)
                //if (sw1.x == "dhm" || sw1.x == "kvn" || sw1.x == "vnc")
                //    if (sw1.y == "dhm" || sw1.y == "kvn" || sw1.y == "vnc")
                        if (ssw2.Item1 == "z32" && ssw2.Item2 == "gfm"
                            && ssw3.Item1 == "z16" && ssw3.Item2 == "mrb"
                            && ssw4.Item1 == "z08" && ssw4.Item2 == "cdj")
                        {
                            swaps.Add((sw1, ssw2, ssw3, ssw4));
                        }
                        else
                        {

                        }
            }
        }
    }
}

foreach(var g in gates.Values)
{
    if(g.Input1 == "x38" && g.Input2 == "y38")
    {
        Console.WriteLine($"{g.Name} {g.GateType}");
    }
}

var ret2 = ret1;
int j = 0;
while(true)
{
    Clear(gates);
    var csw = swaps[j];
    gates[csw.Item1.Item1].SwappedName = csw.Item1.Item2;
    gates[csw.Item1.Item2].SwappedName = csw.Item1.Item1;

    gates[csw.Item2.Item1].SwappedName = csw.Item2.Item2;
    gates[csw.Item2.Item2].SwappedName = csw.Item2.Item1;

    gates[csw.Item3.Item1].SwappedName = csw.Item3.Item2;
    gates[csw.Item3.Item2].SwappedName = csw.Item3.Item1;

    gates[csw.Item4.Item1].SwappedName = csw.Item4.Item2;
    gates[csw.Item4.Item2].SwappedName = csw.Item4.Item1;

    try
    {
        ret2 = Compute(gates, resultGates);

        if (ret2 == expRes)
        {
            Console.WriteLine(j);
            List<string> ret22 = new List<string>
            {
                swaps[j].Item1.Item1, swaps[j].Item1.Item2,
                swaps[j].Item2.Item1, swaps[j].Item2.Item2,
                swaps[j].Item3.Item1, swaps[j].Item3.Item2,
                swaps[j].Item4.Item1, swaps[j].Item4.Item2,
            };

            Console.WriteLine(string.Join(",", ret22.OrderBy(s => s)));
        }
        Console.WriteLine(ret2);
        Console.WriteLine(Convert.ToString(ret2, 2));
        Console.WriteLine(Convert.ToString(expRes, 2));
        Console.WriteLine(Convert.ToString(ret2 ^ expRes, 2));
        Console.WriteLine();
    }
    catch(ArgumentException)
    {
        //Console.WriteLine("cyclic");
    }
    j++;
}

//Console.WriteLine(j - 1);
//List<string> ret22 = new List<string>
//{
//    swaps[j - 1].Item1.Item1, swaps[j - 1].Item1.Item2,
//    swaps[j - 1].Item2.Item1, swaps[j - 1].Item2.Item2,
//    swaps[j - 1].Item3.Item1, swaps[j - 1].Item3.Item2,
//    swaps[j - 1].Item4.Item1, swaps[j - 1].Item4.Item2,
//};

//Console.WriteLine(string.Join(",", ret22.OrderBy(s=>s)));
Console.ReadLine();

class Gate
{
    public string Name { get; set; }
    public string Input1 { get; set; }
    public string Input2 { get; set; }
    public bool InitResult { get; set; }
    public string GateType { get; set; }

    public string SwappedName { get; set; }

    public bool? Result { get; set; }

    public bool? GetResult(Dictionary<string, Gate> gates, int depth)
    {
        if (depth > 47)
            throw new ArgumentException();
        if (Input1 == null && Input2 == null)
        {
            Result = InitResult;
        }
        else
        {
            var g1 = SwappedName == null ? gates[Input1] : gates[gates[SwappedName].Input1];
            var i1 = g1.Result.HasValue ? g1.Result.Value : g1.GetResult(gates, depth+1);
            if (!i1.HasValue)
                return null;

            var g2 = SwappedName == null ? gates[Input2] : gates[gates[SwappedName].Input2];
            var i2 = g2.Result.HasValue ? g2.Result.Value : g2.GetResult(gates, depth + 1);
            if (!i2.HasValue)
                return null;

            switch (GateType)
            {
                case "AND": Result = i1.Value && i2.Value; break;
                case "OR": Result = i1.Value || i2.Value; break;
                case "XOR": Result = i1.Value ^ i2.Value; break;
            }
        }
        return Result;
    }
}