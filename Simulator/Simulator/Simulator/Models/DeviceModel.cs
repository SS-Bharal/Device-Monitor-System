using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Models
{
    public class DeviceModel
    {
        public string DeviceCode { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public int AlarmInterval { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<AlarmDeviceWiseModel> AlarmDeviceWise { get; set; }
    }
}
