namespace API.DataModels
{
    public class DeviceInsertionModel
    {
        public string DeviceCode { get; set; }
        public string DeviceName { get; set; }
        public int AlarmInterval { get; set; }
        public List<Guid> AlarmId { get; set; }
    }
}
