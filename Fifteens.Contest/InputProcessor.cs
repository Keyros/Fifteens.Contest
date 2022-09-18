using System.Diagnostics;

namespace Fifteens.Contest;

public sealed class InputProcessor
{
    private readonly IAsyncEnumerable<int> _data;

    public InputProcessor(IAsyncEnumerable<int> data, int seed, int countToProcess)
    {
        _data = data;
        State = new State($"Chunk {seed}", countToProcess);
    }

    public bool Complete => State.Complete;
    public State State { get; private set; }

    public async Task<IEnumerable<int>> ProcessData()
    {
        var result = new List<int>(State.ToProcess);
        var stopwatch = Stopwatch.StartNew();
        State = State with {Stopwatch = stopwatch};
        await foreach (var item in _data)
        {
            result.Add(item);
            State = State with {Processed = State.Processed + 1};
        }
        stopwatch.Stop();

        return result;
    }
}

public record State(string Name, int ToProcess, int Processed = 0, Stopwatch? Stopwatch = null)
{
    public TimeSpan Elapsed => Stopwatch?.Elapsed ?? TimeSpan.Zero;

    public bool Complete => ToProcess == Processed;

    public double Progress => Processed / (double) ToProcess;

}