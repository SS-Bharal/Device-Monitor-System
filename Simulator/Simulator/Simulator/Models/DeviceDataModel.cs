using System.ComponentModel.DataAnnotations;

namespace Simulator.Models
{
    public class DeviceDataModel
    {
        [Key]
        public Guid Id { get; set; }
        public string DeviceCode { get; set; } = string.Empty;
        public Guid RandomAlarmId { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
