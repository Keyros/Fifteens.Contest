namespace Fifteens.Contest.Enumerators;

public abstract class ContestEnumeratorBase : IAsyncEnumerator<int>, IAsyncEnumerable<int>
{
    private readonly int _finish;

    protected ContestEnumeratorBase((int start, int end) input)
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

        await Task.Yield();
        Current = await GetNext(Current);
        return Current <= _finish;
    }

    protected abstract Task<int> GetNext(int current);


    public int Current { get; private set; }

    public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => this;
}