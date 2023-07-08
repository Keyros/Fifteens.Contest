namespace Fifteens.Contest;

public static class SateNotifier
{
    public static Task GetNotifyTask(this IEnumerable<InputProcessor> inputProcessors) => Task.Factory.StartNew(() =>
    {
        var data = inputProcessors.ToList();
        do
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            PrintSates(data);
        } while (!data.All(x => x.Complete));
    }, TaskCreationOptions.LongRunning);

    public static void PrintSates(this IEnumerable<InputProcessor> inputProcessors)
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
}