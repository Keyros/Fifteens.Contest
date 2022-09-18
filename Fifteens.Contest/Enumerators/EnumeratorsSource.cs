namespace Fifteens.Contest.Enumerators;

public static class EnumeratorsSource
{
    public static IAsyncEnumerable<int> CreateEnumerable((int start, int end) range, int seed)
         => seed % 2 != 0 ? new SimpleContestEnumerator(range) : new ContestEnumerator(range);
}