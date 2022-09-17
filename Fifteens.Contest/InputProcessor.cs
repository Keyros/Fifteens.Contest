using System.Diagnostics;

namespace Fifteens.Contest;

public sealed class InputProcessor
{
    private readonly IAsyncEnumerable<int> _data;
    private readonly string _name;

    public InputProcessor(IAsyncEnumerable<int> data, string name)
    {
        _data = data;
        _name = name;
    }

    public async Task<string> ProcessData()
    {
        Console.WriteLine($"Chunk {_name} started at {DateTime.Now.TimeOfDay}");
        var stopwatch = Stopwatch.StartNew();
        var result = string.Join(',', await _data.ToListAsync());
        Console.WriteLine($"Chunk {_name} completed. Elapsed time: {stopwatch.Elapsed}");
        return result;
    }
}