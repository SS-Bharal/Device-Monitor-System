using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSaveService.Models
{
    public class AppSettingsModel
    {
        public string HostName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
        public string ExchangeName { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
        public string getUrlofRabbitMQ { get; set; }
        public string postUrlForDataSave { get; set; } 
        public int DataSaveTime { get; set; }
    }
}
