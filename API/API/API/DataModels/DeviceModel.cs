using System.ComponentModel.DataAnnotations;

namespace API.DataModels
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

