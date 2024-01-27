namespace Transcript.Service.Ioc
{
    using Microsoft.Extensions.DependencyInjection;
    using Transcript.Service.Processors;

    public static class IoCExtension
    {
        public static IServiceCollection BindBusiness(this IServiceCollection services)
        {
            services.AddScoped<ITranscriptProcessor, TranscriptProcessor>();
            return services;
        }
    }
}