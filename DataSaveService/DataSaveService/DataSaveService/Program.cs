using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using DataSaveService.Interfaces;
using DataSaveService.Services;

namespace DataSaveService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var Host = CreateHostBuilder(args).Build();
                var mainService = Host.Services.GetService<IDataSaveServiceToDb>();
                mainService.DataSaveServiceToDbData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Some error has occurred: {ex.Message}");

            }
        }



        //create host builder
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {

            // Configure Your Services Here...

            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IDataSaveServiceToDb, DataSaveServiceToDb>();


        })
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("C:/Users/VE00YM695/Downloads/MachineTest_SahilBharal/DataSaveService Core/DataSaveService/DataSaveService/appsettings.json", optional: false, reloadOnChange: true);
        });


    }
}
