using Microsoft.Extensions.Configuration;
using Simulator.Interfaces;
using Simulator.Models;
//#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Services
{
    public class ConfigurationService: IConfigurationService
    {
        private readonly AppSettingsModel _appSettings;
        AppSettingsModel IConfigurationService.appSettingModel => _appSettings!;
        public ConfigurationService(IConfiguration configuration)
        {
            _appSettings = configuration.GetSection("AppSettings").Get<AppSettingsModel>()!;
        }
    }
}
