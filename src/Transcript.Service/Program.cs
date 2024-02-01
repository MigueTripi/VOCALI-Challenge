using Transcript.Service.Ioc;
using Transcript.Service.Processors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.BindBusiness())
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
ConfigureAppSettings();

IServiceProvider provider = serviceScope.ServiceProvider;

var transcriptProcessor = provider.GetRequiredService<ITranscriptProcessor>();


var transcriptResult = transcriptProcessor.ProcessVoiceFiles(args[0]).Result;

if (transcriptResult)
{
    Console.WriteLine("Transcript finished");
}
else
{
    Console.WriteLine(transcriptResult);
}

void ConfigureAppSettings()
{
    var builder = new ConfigurationBuilder();
    builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    IConfiguration config = builder.Build();
}