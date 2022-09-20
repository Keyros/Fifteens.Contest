using System.Diagnostics;
using Fifteens.Contest;
using Fifteens.Contest.Enumerators;

Console.WriteLine("Hello, World!");

var processorsCount = Environment.ProcessorCount;
const int countToOutput = 100;

var inputProcessors = Enumerable.Range(1, countToOutput)
    .Chunk(countToOutput / processorsCount)
    .Select(x => (start: x[0], end: x[^1]))
    .Select((x, y) => (enumerable: EnumeratorsSource.CreateEnumerable(x, y), countToPrecess: x.end - x.start + 1))
    .Select((x, y) => new InputProcessor(x.enumerable, y, x.countToPrecess))
    .ToList();

var tasks = inputProcessors.Select(x => x.ProcessData());

var sateNotifier = new SateNotifier(inputProcessors);
var notifyTask = sateNotifier.Run();

var stopWatch = Stopwatch.StartNew();
var enumeration = await Task.WhenAll(tasks);
var result = enumeration.ToList();
stopWatch.Stop();

await notifyTask;

sateNotifier.PrintStates();
Console.WriteLine($"Completed. Elapsed:{stopWatch.Elapsed}");
Console.WriteLine(string.Join(',', result.SelectMany(x => x)));
Console.ReadLine();