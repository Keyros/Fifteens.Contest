namespace Fifteens.Contest.Enumerators;

public interface IEnumeratorSourceFactory
{
    IAsyncEnumerable<int> Create((int start, int end) range, int seed);
}