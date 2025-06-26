namespace API.DataModels
{
    public class DeviceChartGetting
    {
        public string DeviceName { get; set; } = string.Empty;
        public string DeviceCode { get; set; } = string.Empty;
        public string AlarmName { get; set; } = string.Empty;
        public int AlarmCount { get; set; }

    }
}
