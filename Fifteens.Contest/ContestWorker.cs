using System.Diagnostics;
using Fifteens.Contest.Enumerators;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fifteens.Contest;

public sealed class ContestWorker : BackgroundService
{
    private readonly ILogger<ContestWorker> _logger;
    private readonly IEnumeratorSourceFactory _enumeratorSourceFactory;
    private readonly SateNotifier _sateNotifier;
    private readonly IHostApplicationLifetime _applicationLifetime;

    public ContestWorker(ILogger<ContestWorker> logger, IEnumeratorSourceFactory enumeratorSourceFactory,
        SateNotifier sateNotifier,
        IHostApplicationLifetime applicationLifetime)
    {
        _logger = logger;
        _enumeratorSourceFactory = enumeratorSourceFactory;
        _sateNotifier = sateNotifier;
        _applicationLifetime = applicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processorsCount = Environment.ProcessorCount;
        const int countToOutput = 100;

        var inputProcessors = Enumerable.Range(1, countToOutput)
            .Chunk(countToOutput / processorsCount)
            .Select(x => (start: x[0], end: x[^1]))
            .Select((x, y) =>
                (enumerable: _enumeratorSourceFactory.Create(x, y), countToPrecess: x.end - x.start + 1))
            .Select((x, y) => new InputProcessor(x.enumerable, y, x.countToPrecess))
            .ToList();

        var tasks = inputProcessors.Select(x => x.ProcessData());

        var notifyTask = _sateNotifier.Run(inputProcessors);

        var stopWatch = Stopwatch.StartNew();
        var enumeration = await Task.WhenAll(tasks);
        var result = enumeration.ToList();
        stopWatch.Stop();

        await notifyTask;

        _sateNotifier.PrintStates(inputProcessors);
        _logger.LogInformation("Completed. Elapsed:{StopWatchElapsed}", stopWatch.Elapsed);
        _logger.LogInformation("{Data}", string.Join(',', result.SelectMany(x => x)));
        _applicationLifetime.StopApplication();
    }
}