using Fifteens.Contest;
using Fifteens.Contest.Enumerators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


await Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddTransient<IEnumeratorSourceFactory, EnumeratorSourceFactory>();
        services.AddTransient<SateNotifier>();
        services.AddHostedService<ContestWorker>();
    })
    .Build()
    .RunAsync();