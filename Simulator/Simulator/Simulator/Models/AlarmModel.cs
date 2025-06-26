using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.Models
{
    public class AlarmModel
    {
        public Guid AlarmId { get; set; }
        public string AlarmName { get; set; } = string.Empty;
    }
}
