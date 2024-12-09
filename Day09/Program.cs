var input = File.ReadAllText("input.txt").ToList();
long ret1 = 0;
long ret2 = 0;

var files = new List<AocFile>();
var freeSpaces =  new List<AocFreeSpace>();

var memory = new List<long>();
bool free = false;
long idx = 0;
foreach(var ch in input)
{
    var length = int.Parse(ch.ToString());
    if(!free)
    {
        files.Add(new AocFile { Value = idx, Length = length, Pos = memory.Count });
        memory.AddRange(Enumerable.Range(0, length).Select(v=> idx));
        idx++;
    }
    else
    {
        freeSpaces.Add(new AocFreeSpace { Length = length, Pos = memory.Count });
        memory.AddRange(Enumerable.Range(0, length).Select(v => (long)-1));
    }
    free = !free;
}

var memory2 = memory.ToList();

int pos = 0;
int lasPos = memory.Count - 1;
while(lasPos > pos)
{
    if (memory[pos] == -1)
    {
        memory[pos] = memory[lasPos];
        memory[lasPos] = -1;
        lasPos--;
        while (memory[lasPos] == -1)
            lasPos--;
    }

    pos++;
}



foreach (var file in files.Reverse<AocFile>())
{
    var fp = 0;
    AocFreeSpace tomove = null;
    while (true)
    {
        if(freeSpaces[fp].Pos > file.Pos)
        {
            break;
        }
        if (freeSpaces[fp].Length < file.Length)
        {
            fp++;
            if (fp >= freeSpaces.Count)
                break;
        }
        else
        {
            tomove = freeSpaces[fp];
            break;
        }
    }

    if(tomove != null)
    {
        for (int i = 0; i < file.Length; i++)
        {
            memory2[(int)tomove.Pos + i] = file.Value;

            memory2[(int)file.Pos + i] = -1;
        }
        tomove.Pos = tomove.Pos + file.Length;
        tomove.Length = tomove.Length - file.Length;
    }
}

ret1 = memory.Where(v => v >= 0).Select((i, v) => i * v).Sum();
ret2 = memory2.Select((v, i) => v == -1 ? 0 : i * v).Sum();

Console.WriteLine(ret1);
Console.WriteLine(ret2);

Console.ReadLine();

class AocFile
{
    public long Value { get; set; }
    public long Length { get; set; }
    public long Pos { get; set; }
}

class AocFreeSpace
{
    public long Length { get; set; }
    public long Pos { get; set; }
}