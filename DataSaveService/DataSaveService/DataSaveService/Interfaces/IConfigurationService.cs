using DataSaveService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaveService.Interfaces
{
    public interface IConfigurationService
    {
        AppSettingsModel appSettingModel { get; }

    }
}
