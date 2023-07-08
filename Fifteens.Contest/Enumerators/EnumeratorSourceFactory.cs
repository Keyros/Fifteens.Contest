namespace Fifteens.Contest.Enumerators;

internal sealed class EnumeratorSourceFactory : IEnumeratorSourceFactory
{
    public IAsyncEnumerable<int> Create((int start, int end) range, int seed) =>
        seed % 2 != 0 ? new SimpleContestEnumerator(range) : new ContestEnumerator(range);
}