using System.ComponentModel.DataAnnotations;

namespace API.DataModels
{
    public class DeviceDataModel
    {
        [Key]
        public Guid Id { get; set; }
        public string DeviceCode { get; set; }
        public Guid RandomAlarmId { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
