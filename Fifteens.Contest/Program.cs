// See https://aka.ms/new-console-template for more information


using System.Diagnostics;

Console.WriteLine("Hello, World!");

var processorsCount = Environment.ProcessorCount;
const int countToOutput = 100;

var tasks = Enumerable.Range(1, countToOutput)
    .Chunk(countToOutput / processorsCount)
    .Select((x) => (x[0], x[^1]))
    .Select(CreateSimpleEnumerator)
    .Select(ProcessData);

Console.WriteLine(string.Join(',', await Task.WhenAll(tasks)));

async Task<string> ProcessData(IAsyncEnumerable<int> data, int i)
{
    Console.WriteLine($"Chunk {i} started at {DateTime.Now.TimeOfDay}");
    var stopwatch = Stopwatch.StartNew();
    var result = string.Join(',', await data.ToListAsync());
    Console.WriteLine($"Chunk {i} completed. Elapsed time: {stopwatch.Elapsed}");
    return result;
}

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


IAsyncEnumerable<int> CreateSimpleEnumerator((int start, int end) input) => Enumerable.Range(input.start, input.end - input.start + 1).ToAsyncEnumerable();
Console.WriteLine("Completed.");
Console.ReadLine();