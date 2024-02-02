namespace Transcript.Service.Ioc
{
    using Microsoft.Extensions.DependencyInjection;
    using Transcript.Service.Helpers;
    using Transcript.Service.Interfaces;
    using Transcript.Service.Processors;
    using Transcript.Service.Services;

    public static class IoCExtension
    {
        public static IServiceCollection BindBusiness(this IServiceCollection services)
        {
            services.AddScoped<ITranscriptProcessor, TranscriptProcessor>();
            services.AddScoped<IConfigurationHelper, ConfigurationHelper>();
            services.AddScoped<IInvoxService, InvoxService>();
            services.AddSingleton<IFileHelper, FileHelper>();
            return services;
        }
    }
}