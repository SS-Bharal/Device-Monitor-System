namespace Simulator.Models
{
    public class AMQPSettingsModel
    {
        public Guid id { get; set; }
        public string hostName { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public int port { get; set; }

    }
}
