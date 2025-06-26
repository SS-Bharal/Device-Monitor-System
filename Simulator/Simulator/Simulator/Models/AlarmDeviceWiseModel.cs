using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Models
{
    public class AlarmDeviceWiseModel
    {
        public Guid Id { get; set; }
        public string DeviceCode { get; set; } = string.Empty;
        public Guid AlarmId { get; set; }

        public DeviceModel Device { get; set; }
        public AlarmModel Alarm { get; set; }
    }
}
