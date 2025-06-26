using DataSaveService.Interfaces;
using DataSaveService.Models;
using RabbitMQ.Client;
using System;
using System.Text;
#nullable disable
using RabbitMQ.Client.Exceptions;
using Serilog;
using RabbitMQ.Client.Events;
using System.Text.Json;


namespace DataSaveService.Services
{
    public class DataSaveServiceToDb: IDataSaveServiceToDb
    {

        private readonly IConfigurationService _configuration;

        //Dependency Injection of ConfigurationService Interface
        public DataSaveServiceToDb(IConfigurationService configuration)
        {
            _configuration = configuration;

            Log.Logger = new LoggerConfiguration()
           //.WriteTo.Console()
           .WriteTo.File("./Logging/log.txt")
           .CreateLogger();

        }

        public async void DataSaveServiceToDbData()
        {

            try
            {
                //GET API call for RabbitMQ configuration
                //HttpClient clientAQMP = new HttpClient();
                //var responseAQMP = await clientAQMP.GetAsync(_configuration.appSettingModel.getUrlofRabbitMQ);
                //responseAQMP.EnsureSuccessStatusCode();
                //string responseBodyAQMP = await responseAQMP.Content.ReadAsStringAsync();
                //var RabbitMQSettings = JsonSerializer.Deserialize<AMQPSettingsModel>(responseBodyAQMP);

                //HttpClient clientAQMP = new HttpClient();
                //var responseAQMP = await clientAQMP.GetAsync("https://localhost:44356/api/Mt/getamqpsettings");
                //responseAQMP.EnsureSuccessStatusCode();
                //string responseBodyAQMP = await responseAQMP.Content.ReadAsStringAsync();
                //ServiceResponseModel RabbitMQSettings = JsonSerializer.Deserialize<ServiceResponseModel>(responseBodyAQMP);
                //AMQPSettingsModel RbtMQSettings = JsonSerializer.Deserialize<AMQPSettingsModel>(RabbitMQSettings.result);

                var factory = new ConnectionFactory { HostName = _configuration.appSettingModel.HostName, UserName = _configuration.appSettingModel.UserName, Password = _configuration.appSettingModel.Password, Port = _configuration.appSettingModel.Port };
                //var factory = new ConnectionFactory { HostName = RbtMQSettings.hostName, UserName = RbtMQSettings.userName, Password = RbtMQSettings.password, Port = RbtMQSettings.port};

                while (true)
                {
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        string queueName = _configuration.appSettingModel.Queue;
                        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                        var consumer = new EventingBasicConsumer(channel);

                        consumer.Received += (model, ea) =>
                        {
                            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                            

                           
                            // Deserialize the message to your data model
                            var data = JsonSerializer.Deserialize<DeviceDataModel>(message);
                            //var data = JsonSerializer.Deserialize<DeviceData>(message);

                            DeviceDataModel dData = new DeviceDataModel();
                            dData.DeviceCode = data.DeviceCode;
                            dData.RandomAlarmId = data.RandomAlarmId;
                            dData.DateCreated = data.DateCreated;



                            try
                            {
                                // Make an API call to send data
                                using (var httpClient = new HttpClient())
                                {
                                  
                                
                                   var jsonData = JsonSerializer.Serialize(dData);
                                   var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                                   var response = httpClient.PostAsync("https://localhost:44356/api/Mt/savedevice", content).Result;
                                    Console.WriteLine("Hello Packets Sent...");

                                    if (response.IsSuccessStatusCode)
                                   {
                                       Console.WriteLine("Data sent successfully to API.");
                                   }
                                   else
                                   {
                                       Console.WriteLine($"Error sending data to API: {response.StatusCode}");
                                   }

                                    System.Threading.Thread.Sleep(_configuration.appSettingModel.DataSaveTime);

                                }
                            }
                            //Catching HttpRequestException 
                            catch (HttpRequestException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;

                                Console.WriteLine($"{DateTime.Now} | Simulator | HTTP Request Error: {ex.Message}");
                                Log.Information($"Simulator | Failed to connect to RabbitMQ: {ex.Message}");

                                Console.ResetColor();

                            }
                            //catching TaskCanceledException 
                            catch (TaskCanceledException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;

                                Console.WriteLine($"{DateTime.Now} | Simulator | Request Timeout Error: {ex.Message}");
                                Log.Information($"Simulator | Failed to connect to RabbitMQ: {ex.Message}");

                                Console.ResetColor();

                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;

                                Console.WriteLine($"{DateTime.Now} | Simulator | An error occurred: {ex.Message}");
                                Log.Information($"Simulator | Failed to connect to RabbitMQ: {ex.Message}");

                                Console.ResetColor();
                            }
                        };

                        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                    }

                }
            }
            catch (BrokerUnreachableException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine($"{DateTime.Now} | Simulator | Failed to connect to RabbitMQ: {ex.Message}");
                Log.Information($"Simulator | Failed to connect to RabbitMQ: {ex.Message}");

                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine($"{DateTime.Now} | Simulator | Failed to connect to RabbitMQ: {ex.Message}");
                Log.Information($" Simulator | Failed to connect to RabbitMQ: {ex.Message}");

                Console.ResetColor();

            }
           
        }


    }
}




