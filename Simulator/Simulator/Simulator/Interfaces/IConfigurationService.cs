using Microsoft.Extensions.Configuration;
using Simulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Interfaces
{
    public interface IConfigurationService
    {
       AppSettingsModel appSettingModel { get; }

    }
}
