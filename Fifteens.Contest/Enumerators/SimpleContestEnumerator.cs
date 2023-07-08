namespace Fifteens.Contest.Enumerators;

internal sealed class SimpleContestEnumerator : ContestEnumeratorBase
{
    public SimpleContestEnumerator((int start, int end) input) : base(input)
    {
    }

    protected override async Task<int> GetNext(int current)
    {
        await Task.Delay(1000);
        return current + 1;
    }
}