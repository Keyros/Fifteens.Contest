using Fifteens.Contest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


await Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => { services.AddHostedService<ContestWorker>(); })
    .Build()
    .RunAsync();