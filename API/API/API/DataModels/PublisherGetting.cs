namespace API.DataModels
{
    public class PublisherGetting
    {
        public string DeviceCode { get; set; }
        public int AlarmInterval { get; set; }
        public List<Guid> AlarmsList { get; set; }

    }
}
