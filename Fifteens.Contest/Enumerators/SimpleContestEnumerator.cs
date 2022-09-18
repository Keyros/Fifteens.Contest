namespace Fifteens.Contest.Enumerators;

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
        if (Current == _finish)
        {
            return false;
        }
        await Task.Delay(1000);
        Current++;
        return Current <= _finish;
    }

    public int Current { get; private set; }

    public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => this;
}