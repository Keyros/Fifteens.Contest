namespace Fifteens.Contest;

public sealed class ContestEnumerator : IAsyncEnumerator<int>, IAsyncEnumerable<int>
{
    private readonly int _finish;
    private readonly Random _rnd;

    public ContestEnumerator((int start, int end) input)
    {
        _finish = input.end;
        Current = input.start - 1;
        _rnd = new Random();
    }


    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public async ValueTask<bool> MoveNextAsync()
    {
        await Task.Yield();
        int number;
        do
        {
            number = _rnd.Next();
        } while (number != Current);

        Current++;

        return Current < _finish + 1;
    }

    public int Current { get; private set; }

    public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => this;
}