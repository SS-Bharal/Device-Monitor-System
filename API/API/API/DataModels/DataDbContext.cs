using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.DataModels
{
    public class DataDbContext : DbContext
    {
        //public DbSet<ModelName> TableName { get; set; }

        public DbSet<DeviceModel> Devices { get; set; }
        public DbSet<AlarmDeviceWiseModel> AlarmDeviceWise { get; set; }
        public DbSet<AlarmModel> Alarms { get; set; }
        public DbSet<DeviceDataModel> DeviceData { get; set; }
        public DbSet<AMQPSettingsModel> AMQPSettings { get; set; }


        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure your Model Mappings and Relationship Here 

            //modelBuilder.Entity<DevicesModel>()
            //    .HasIndex(e => e.DeviceName)
            //    .IsUnique();

            //modelBuilder.Entity<DevicesModel>()
            //    .HasIndex(e => e.DeviceCode)
            //    .IsUnique();

            modelBuilder.Entity<DeviceModel>().HasKey(d => d.DeviceCode);
            modelBuilder.Entity<AlarmModel>().HasKey(a => a.AlarmId);

            // Define relationships
            modelBuilder.Entity<AlarmDeviceWiseModel>()
                .HasKey(adw => adw.Id);
            modelBuilder.Entity<AlarmDeviceWiseModel>()
                .HasOne(adw => adw.Device)
                .WithMany(device => device.AlarmDeviceWise)
                .HasForeignKey(adw => adw.DeviceCode);

            modelBuilder.Entity<AlarmDeviceWiseModel>()
                .HasOne(adw => adw.Alarm)
                .WithMany()
                .HasForeignKey(adw => adw.AlarmId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
