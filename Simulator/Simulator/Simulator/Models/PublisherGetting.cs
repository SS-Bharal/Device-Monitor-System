namespace API.DataModels
{
    public class PublisherGetting
    {
        public string deviceCode { get; set;} = string.Empty;
        public int alarmInterval { get; set; }
        public List<Guid> alarmsList { get; set; }


    }
}
