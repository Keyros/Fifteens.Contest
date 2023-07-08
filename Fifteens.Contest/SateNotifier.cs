namespace Fifteens.Contest;

public sealed class SateNotifier
{
    private readonly List<InputProcessor> _inputProcessors;

    public SateNotifier(List<InputProcessor> inputProcessors)
    {
        _inputProcessors = inputProcessors;
    }

    public Task Run() => Task.Factory.StartNew(() =>
    {
        do
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            PrintSates(_inputProcessors);
        } while (!_inputProcessors.All(x => x.Complete));
    }, TaskCreationOptions.LongRunning);

    private static void PrintSates(IEnumerable<InputProcessor> inputProcessors)
    {
        Console.Clear();
        Console.WriteLine(new string('*', 80));
        Console.WriteLine($"State info {DateTime.Now.TimeOfDay}");
        var states = inputProcessors
            .OrderByDescending(x => x.State.Complete)
            .ThenBy(x => x.State.Elapsed)
            .Select(x => CreateString(x.State));
        var statesString = string.Join(Environment.NewLine, states);
        Console.WriteLine(statesString);
        Console.WriteLine(new string('*', 80));
    }

    private static string CreateString(State state)
        => $"Name: {state.Name}, ToProcess = {state.ToProcess}, " +
           $"Processed = {state.Processed}\t, Elapsed = {state.Elapsed}, " +
           $"Progress = {state.Progress * 100}%\t, Complete = {state.Complete}";

    public void PrintStates() => PrintSates(_inputProcessors);
}