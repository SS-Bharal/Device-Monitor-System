using System.Security.Claims;

namespace API.DataModels
{
    public class AlarmDeviceWiseModel
    {
        public Guid Id { get; set; }
        public string DeviceCode { get; set; }
        public Guid AlarmId { get; set; }

        public DeviceModel Device { get; set; }
        public AlarmModel Alarm { get; set; }
    }
}
