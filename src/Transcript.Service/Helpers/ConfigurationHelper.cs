namespace Transcript.Service.Helpers
{
    using Microsoft.Extensions.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using Transcript.Service.Interfaces;

    [ExcludeFromCodeCoverage]
    public class ConfigurationHelper : IConfigurationHelper
    {
        private readonly IConfiguration configuration;

        public ConfigurationHelper(IConfiguration config)
        {
            configuration = config;
        }

        public virtual int GetConfigValueWithDefault(string key, int defaultValue)
        {
            return int.TryParse(configuration[key], out int value) ? value : defaultValue;
        }
    }
}
