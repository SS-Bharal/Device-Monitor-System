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
using Simulator.Interfaces;
using Simulator.Services;

namespace Simulator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {   

            try
            {
                var Host = CreateHostBuilder(args).Build();
                var mainService = Host.Services.GetService<IDataSimulatorService>();
                //mainService.GenerateData().Wait();

                Task generateDataTask = Task.Run(() => mainService.GenerateData());
                //Task publishDataTask = Task.Run(() => mainService.PublishData());

                // Wait for both tasks to complete
                await Task.WhenAll(generateDataTask);
                //mainService.PublishData();
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
            services.AddSingleton<IDataSimulatorService, DataSimulatorService>();

           
        })
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("C:/Users/VE00YM695/Downloads/Machine_Test_Final_SahilBharal/Simulator/Simulator/Simulator/appsettings.json", optional: false, reloadOnChange: true);
        });


    }
}
