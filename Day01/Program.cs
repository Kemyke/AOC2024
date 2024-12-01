var input = File.ReadAllLines("input.txt");
var l = input.Select(x => x.Split("   ")).Select(x => (int.Parse(x[0]), int.Parse(x[1])));
var l1o = l.Select(x => x.Item1).OrderBy(l => l);
var l2o = l.Select(x => x.Item2).OrderBy(l => l);

var ret1 = l1o.Zip(l2o).Sum(x => Math.Abs(x.First - x.Second));
var ret2 = l1o.Sum(x=>x * l2o.Count(y => y == x));

Console.ReadLine();