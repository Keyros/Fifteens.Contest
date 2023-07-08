namespace Fifteens.Contest.Enumerators;

internal sealed class ContestEnumerator : ContestEnumeratorBase
{
    public ContestEnumerator((int start, int end) input) : base(input)
    {
    }

    protected override async Task<int> GetNext(int current)
    {
        var numberTask = await Task.WhenAny(Enumerable.Range(0, 1).Select(_ => Test(current)));
        return await numberTask;
    }

    private static Task<int> Test(int current)
    {
        return Task.Factory.StartNew(() =>
        {
            int number;
            do
            {
                number = Random.Shared.Next();
            } while (number != current + 1);

            return number;
        }, TaskCreationOptions.LongRunning);
    }
}