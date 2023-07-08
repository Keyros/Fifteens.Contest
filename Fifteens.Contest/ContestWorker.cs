using System.Diagnostics;
using Fifteens.Contest.Enumerators;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fifteens.Contest;

public sealed class ContestWorker : BackgroundService
{
    private readonly ILogger<ContestWorker> _logger;

    public ContestWorker(ILogger<ContestWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processorsCount = Environment.ProcessorCount;
        const int countToOutput = 100;

        var inputProcessors = Enumerable.Range(1, countToOutput)
            .Chunk(countToOutput / processorsCount)
            .Select(x => (start: x[0], end: x[^1]))
            .Select((x, y) =>
                (enumerable: EnumeratorsSource.CreateEnumerable(x, y), countToPrecess: x.end - x.start + 1))
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
        _logger.LogInformation("Completed. Elapsed:{StopWatchElapsed}", stopWatch.Elapsed);
        _logger.LogInformation("{Data}", string.Join(',', result.SelectMany(x => x)));
    }
}