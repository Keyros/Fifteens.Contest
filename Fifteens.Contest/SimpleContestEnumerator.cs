namespace Fifteens.Contest;

public sealed class SimpleContestEnumerator : IAsyncEnumerator<int>, IAsyncEnumerable<int>
{
    private readonly int _finish;

    public SimpleContestEnumerator((int start, int end) input)
    {
        _finish = input.end;
        Current = input.start - 1;
    }


    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public async ValueTask<bool> MoveNextAsync()
    {
        await Task.Yield();
        Current++;
        return Current < _finish + 1;
    }

    public int Current { get; private set; }

    public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => this;
}