namespace API.DataModels
{
    public class MasterDataSeeder
    {
        public static void SeedMasterData(DataDbContext context)
        {
            if (!context.AMQPSettings.Any())
            {
                var amqpSettings = new List<AMQPSettingsModel>
            {
                new AMQPSettingsModel
                {
                    HostName = "10.167.96.47",
                    UserName = "sahil",
                    Password = "sahil",
                    Port = 5672
                }
            };

                context.AMQPSettings.AddRange(amqpSettings);
                context.SaveChanges();
            }

            if (!context.Alarms.Any())
            {
                var alarmModels = new List<AlarmModel>
            {
                new AlarmModel { AlarmName = "energy saver on" },
                new AlarmModel { AlarmName = "cooling pump not on" },
                new AlarmModel { AlarmName = "system brake down" },
                new AlarmModel { AlarmName = "cycle time high" },
               
            };

            context.Alarms.AddRange(alarmModels);
                context.SaveChanges();
            }
        }
    }
}
