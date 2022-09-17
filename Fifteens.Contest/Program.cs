// See https://aka.ms/new-console-template for more information

using Fifteens.Contest;

Console.WriteLine("Hello, World!");

var processorsCount = Environment.ProcessorCount;
const int countToOutput = 100;

var inputProcessors = Enumerable.Range(1, countToOutput)
    .Chunk(countToOutput / processorsCount)
    .Select((x) => (start: x[0], end: x[^1]))
    .Select(x => new SimpleContestEnumerator(x))
    .Select((x, y) => new InputProcessor(x, y.ToString()));

var tasks = inputProcessors.Select(x => x.ProcessData());

Console.WriteLine(string.Join(',', await Task.WhenAll(tasks)));


IAsyncEnumerable<int> CreateSimpleEnumerator((int start, int end) input) =>
    Enumerable.Range(input.start, input.end - input.start + 1).ToAsyncEnumerable();

Console.WriteLine("Completed.");
Console.ReadLine();