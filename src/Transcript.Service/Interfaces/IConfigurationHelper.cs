using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transcript.Service.Interfaces
{
    public interface IConfigurationHelper
    {
        int GetConfigValueWithDefault(string key, int defaultValue);
    }
}
