// See https://aka.ms/new-console-template for more information


using System.Diagnostics;

Console.WriteLine("Hello, World!");

var processorsCount = Environment.ProcessorCount;
var countToOutput = 100;

var tasks = Enumerable.Range(1, countToOutput)
    .Chunk(countToOutput / processorsCount)
    .Select(CreateEnumerator)
    .Select(ProcessData);

foreach (var data in await Task.WhenAll(tasks))
{
    Console.WriteLine(data);
}

async Task<string> ProcessData(IAsyncEnumerable<int> data, int i)
{
    Console.WriteLine($"Chunk {i} started");
    var stopwatch = Stopwatch.StartNew();
    var result = string.Join(',', await data.ToListAsync());
    Console.WriteLine($"Chunk {i} completed: {stopwatch.Elapsed}");
    return result;

}

async IAsyncEnumerable<int> CreateEnumerator(IEnumerable<int> data)
{
    var rnd = new Random();
    var dataAsList = data.ToList();
    var start = dataAsList.Min();
    var finish = dataAsList.Max();
    do
    {
        var number = rnd.Next();
        if (number == start)
        {
            yield return number;
            start++;
        }

    } while (start < finish);

}

IAsyncEnumerable<int> CreateSimpleEnumerator(IEnumerable<int> data) => data.ToAsyncEnumerable();

Console.ReadLine();