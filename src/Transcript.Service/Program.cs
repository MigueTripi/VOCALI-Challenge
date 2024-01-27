using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transcript.Service.Ioc;
using Transcript.Service.Processors;


var serviceScope = ConfigureAppIoC();
ConfigureAppSettings();

IServiceProvider provider = serviceScope.ServiceProvider;

var transcriptProcessor = provider.GetRequiredService<ITranscriptProcessor>();


List<string> list = new List<string>();


IServiceScope ConfigureAppIoC()
{
    using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.BindBusiness())
    .Build();

    return host.Services.CreateScope();
}

void ConfigureAppSettings()
{
    var builder = new ConfigurationBuilder();
    builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    IConfiguration config = builder.Build();

    var batchSize = config["BatchSize"];

    Console.WriteLine($"Batch Size {batchSize}");
}