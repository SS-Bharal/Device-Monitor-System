namespace API.DataModels
{
    public class AMQPSettingsModel
    {
        public Guid id { get; set; }
        public string HostName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }

    }
}
