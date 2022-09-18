using Fifteens.Contest;
using Fifteens.Contest.Enumerators;

Console.WriteLine("Hello, World!");

var processorsCount = Environment.ProcessorCount * 2;
const int countToOutput = 100;

var inputProcessors = Enumerable.Range(1, countToOutput)
    .Chunk(countToOutput / processorsCount)
    .Select((x) => (start: x[0], end: x[^1]))
    .Select(EnumeratorsSource.CreateEnumerable)
    .Select((x, y) => new InputProcessor(x, y.ToString()));

var tasks = inputProcessors.Select(x => x.ProcessData());


Console.WriteLine(string.Join(',', await Task.WhenAll(tasks)));

Console.WriteLine("Completed.");
Console.ReadLine();