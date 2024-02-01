using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transcript.Service.Interfaces;

namespace Transcript.Service
{
    internal class ConfigurationHelper : IConfigurationHelper
    {
        private readonly IConfiguration configuration;

        public ConfigurationHelper(IConfiguration config)
        {
            configuration = config;
        }

        public int GetConfigValueWithDefault(string key, int defaultValue)
        {
            return int.TryParse(configuration[key], out int value) ? value : defaultValue;
        }
    }
}
