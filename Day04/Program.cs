var input = File.ReadAllLines("input.txt");
var ret1 = 0;
var ret2 = 0;

for (int y = 0;  y < input.Length; y++)
{
    var line = input[y];

    for (int x = 0; x < line.Length; x++)
    {
        if (line[x] == 'X')
        {
            if (SearchRight(input, x, y))
                ret1++;
            if (SearchLeft(input, x, y))
                ret1++;
            if (SearchUp(input, x, y))
                ret1++;
            if (SearchDown(input, x, y))
                ret1++;
            if (SearchUpRight(input, x, y))
                ret1++;
            if (SearchUpLeft(input, x, y))
                ret1++;
            if (SearchDownRight(input, x, y))
                ret1++;
            if (SearchDownLeft(input, x, y))
                ret1++;
        }

        if (line[x] == 'M')
        {
            if (SearchXLeft(input, x, y))
                ret2++;
            if (SearchXRight(input, x, y))
                ret2++;
            if (SearchXUp(input, x, y))
                ret2++;
            if (SearchXDown(input, x, y))
                ret2++;
        }
    }
}


Console.WriteLine(ret1);
Console.WriteLine(ret2);
Console.ReadLine();

static bool SearchXLeft(string[] input, int x, int y)
{
    var ret = y < input.Length - 2 && x < input[y].Length - 2 && input[y + 1][x + 1] == 'A' && input[y][x + 2] == 'S' && input[y + 2][x] == 'M' && input[y + 2][x + 2] == 'S';
    if(ret)
    {
        Console.WriteLine($"left {y} {x}");

    }
    return ret;
}

static bool SearchXRight(string[] input, int x, int y)
{
    var ret = y < input.Length - 2 && x > 1 && input[y + 1][x - 1] == 'A' && input[y][x - 2] == 'S' && input[y + 2][x] == 'M' && input[y + 2][x - 2] == 'S';
    if(ret) 
    Console.WriteLine($"right {y} {x}");
    return ret;
}

static bool SearchXDown(string[] input, int x, int y)
{
    var ret = y > 1 && x < input[y].Length - 2 && input[y - 1][x + 1] == 'A' && input[y - 2][x + 2] == 'S' && input[y][x + 2] == 'M' && input[y - 2][x] == 'S';
    if(ret)
        Console.WriteLine($"down {y} {x}");
    return ret;
}

static bool SearchXUp(string[] input, int x, int y)
{
    var ret = y < input.Length - 2 && x < input[y].Length - 2 && input[y + 1][x + 1] == 'A' && input[y + 2][x + 2] == 'S' && input[y][x + 2] == 'M' && input[y + 2][x] == 'S';
    if(ret)
        Console.WriteLine($"up {y} {x}");

    return ret;
}

static bool SearchRight(string[] input, int x, int y)
{
    return x < input[y].Length - 3 && input[y][x + 1] == 'M' && input[y][x + 2] == 'A' && input[y][x + 3] == 'S';
}

static bool SearchLeft(string[] input, int x, int y)
{
    return x > 2 && input[y][x - 1] == 'M' && input[y][x - 2] == 'A' && input[y][x - 3] == 'S';
}

static bool SearchUp(string[] input, int x, int y)
{
    return y > 2 && input[y - 1][x] == 'M' && input[y - 2][x] == 'A' && input[y - 3][x] == 'S';
}

static bool SearchDown(string[] input, int x, int y)
{
    return y < input.Length - 3 && input[y + 1][x] == 'M' && input[y + 2][x] == 'A' && input[y + 3][x] == 'S';
}

static bool SearchUpRight(string[] input, int x, int y)
{
    return y > 2 && x < input[y].Length - 3 && input[y - 1][x + 1] == 'M' && input[y - 2][x + 2] == 'A' && input[y - 3][x + 3] == 'S';
}

static bool SearchUpLeft(string[] input, int x, int y)
{
    return y > 2 && x > 2 && input[y - 1][x - 1] == 'M' && input[y - 2][x - 2] == 'A' && input[y - 3][x - 3] == 'S';
}

static bool SearchDownRight(string[] input, int x, int y)
{
    return y < input.Length - 3 && x < input[y].Length - 3 && input[y + 1][x + 1] == 'M' && input[y + 2][x + 2] == 'A' && input[y + 3][x + 3] == 'S';
}

static bool SearchDownLeft(string[] input, int x, int y)
{
    return y < input.Length - 3 && x > 2 && input[y + 1][x - 1] == 'M' && input[y + 2][x - 2] == 'A' && input[y + 3][x - 3] == 'S';
}