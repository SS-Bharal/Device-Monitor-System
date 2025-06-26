namespace API.DataModels
{
    public class DeviceGetting
    {
        public string DeviceName { get; set; } = string.Empty;
        public string DeviceCode { get; set; } = string.Empty;
        public string AlarmName { get; set; } = string.Empty;
        
        public DateTime DateTime { get; set; }
    }
}
