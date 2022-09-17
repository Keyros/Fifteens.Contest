// See https://aka.ms/new-console-template for more information

using Fifteens.Contest;

Console.WriteLine("Hello, World!");

var processorsCount = Environment.ProcessorCount;
const int countToOutput = 100;

var inputProcessors = Enumerable.Range(1, countToOutput)
    .Chunk(countToOutput / processorsCount)
    .Select((x) => (x[0], x[^1]))
    .Select(CreateSimpleEnumerator)
    .Select((x, y) => new InputProcessor(x, y.ToString()));

var tasks = inputProcessors.Select(x => x.ProcessData());

Console.WriteLine(string.Join(',', await Task.WhenAll(tasks)));


async IAsyncEnumerable<int> CreateEnumerator((int start, int end) input)
{
    var rnd = new Random();
    var start = input.start;
    var finish = input.end;
    do
    {
        await Task.Yield();
        var number = rnd.Next(start, finish);
        if (number == start)
        {
            yield return number;
            start++;
        }
    } while (start < finish + 1);
}


IAsyncEnumerable<int> CreateSimpleEnumerator((int start, int end) input) =>
    Enumerable.Range(input.start, input.end - input.start + 1).ToAsyncEnumerable();

Console.WriteLine("Completed.");
Console.ReadLine();