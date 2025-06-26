using Simulator.Interfaces;
using Simulator.Models;
using RabbitMQ.Client;
using System;
using System.Text;
//using Newtonsoft.Json;
#nullable disable
using System.Text.Json;
using RabbitMQ.Client.Exceptions;
using Serilog;
using System.Net.Http.Headers;
using System.Collections;
using API.DataModels;

namespace Simulator.Services
{
    public class DataSimulatorService : IDataSimulatorService
    {

        private readonly IConfigurationService _configuration;

        //Dependency Injection of ConfigurationService Interface
        public DataSimulatorService(IConfigurationService configuration)
        {
            _configuration = configuration;


            //Error Logging to file
            #region Logging
            //Logging
            Log.Logger = new LoggerConfiguration()
                //.WriteTo.Console()
                .WriteTo.File("./Logging/log.txt")
                .CreateLogger();   //.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day) // Output log messages to a file
            #endregion

        }


        //public AMQPSettingsModel dList;
        public AMQPSettingsModel _AMQPSettings;
        public async Task GenerateData()
        {
            
            try
            {
                //GET API call for RabbitMQ configuration
                HttpClient clientAQMP = new HttpClient();
                var responseAQMP = await clientAQMP.GetAsync(_configuration.appSettingModel.getUrlofRabbitMQ);
                responseAQMP.EnsureSuccessStatusCode();
                string responseBodyAQMP = await responseAQMP.Content.ReadAsStringAsync();
                ServiceResponseModel RabbitMQSettings = JsonSerializer.Deserialize<ServiceResponseModel>(responseBodyAQMP);
                AMQPSettingsModel RbtMQSettings = JsonSerializer.Deserialize<AMQPSettingsModel>(RabbitMQSettings.result);
                _AMQPSettings = RbtMQSettings;

                while (true)
                {
                    //Getting Data from Devices
                    HttpClient client = new HttpClient();
                    var response = await client.GetAsync(_configuration.appSettingModel.getUrlofDevices);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ServiceResponseModel DeviceListData = JsonSerializer.Deserialize<ServiceResponseModel>(responseBody);

                    List<PublisherGetting> DeviceList = JsonSerializer.Deserialize<List<PublisherGetting>>(DeviceListData.result);
                    //DeviceList.ForEach(device =>
                    //{
                    //    GenerateDeviceData(device);

                    //});


                    Parallel.ForEach(DeviceList, device =>
                    {
                        GenerateDeviceData(device);
                    });


                    //Console.WriteLine("Next Line...");
                    Thread.Sleep(_configuration.appSettingModel.fetchDeviceDataTime);

                }
            }
            catch (HttpRequestException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine($"{DateTime.Now} | Simulator | HTTP Request Error: {ex.Message}");
                Log.Information($"Simulator | Failed to connect to RabbitMQ: {ex.Message}");

                Console.ResetColor();


            }
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
        }



        public void GenerateDeviceData(PublisherGetting device)
        {


            var factory = new ConnectionFactory { HostName = _AMQPSettings.hostName, UserName = _AMQPSettings.userName, Password = _AMQPSettings.password, Port = _AMQPSettings.port };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            string exchangeName = _configuration.appSettingModel.ExchangeName;
            string queueName = _configuration.appSettingModel.Queue;

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");

            try
            {

                //using ()
                //      using ()
                //{

                int tCount =0;
                int tempCount = 0;
                int flag = 0;

                while (true)
                {
                    flag = 0;
                    tempCount++;
                    DeviceDataModel deviceData = new DeviceDataModel();
                    DeviceDataModel deviceDataEmpty = new DeviceDataModel();

                    List<Guid> alarmData = device.alarmsList;

                    // Create a random number generator
                    Random random = new Random();
                    int randomIndex = random.Next(0, alarmData.Count);
                    Guid randomlySelectedAlarmId = alarmData[randomIndex];


                    deviceData.DeviceCode = device.deviceCode;
                    deviceData.RandomAlarmId = randomlySelectedAlarmId;


                    deviceDataEmpty.DeviceCode = device.deviceCode;
                    deviceDataEmpty.RandomAlarmId = Guid.Empty;

                    deviceData.DateCreated = DateTime.Now;
                    deviceDataEmpty.DateCreated = DateTime.Now;


                    string json = JsonSerializer.Serialize(deviceData);
                    string jsonEmpty = JsonSerializer.Serialize(deviceDataEmpty);


                    if (_configuration.appSettingModel.PublishRequestTime * tempCount > device.alarmInterval * 1000)
                    {
                        tempCount = 0;
                        flag = 1;



                        if (connection.IsOpen)
                        {

                            try
                            {

                                byte[] body = Encoding.UTF8.GetBytes(json);

                                channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);


                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine($"{deviceData.DeviceCode},{deviceData.RandomAlarmId},{deviceData.DateCreated}");
                                Console.ResetColor();

                                flag = 1;
                                tCount++;
                                if(tCount == 2) { break; }



                            }
                            catch (Exception)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($"{DateTime.Now} | Simulator | Failed to publish message");
                                Log.Information($"Simulator | Failed to publish message");

                                //messageQueue.Clear();
                                Console.ResetColor();

                            }

                            //System.Threading.Thread.Sleep(2000);

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;

                            Console.WriteLine($"{DateTime.Now} | Simulator | Connection to RabbitMQ is Closed.  Reconnecting...");
                            Log.Information($"Simulator | Connection to RabbitMQ is Closed.  Reconnecting...");

                            Console.ResetColor();

                        }


                    }

                    if (flag != 1)
                    {
                        if (connection.IsOpen)
                        {

                            try
                            {

                                byte[] body = Encoding.UTF8.GetBytes(jsonEmpty);

                                channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);


                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine($"{deviceDataEmpty.DeviceCode},{deviceDataEmpty.RandomAlarmId},{deviceDataEmpty.DateCreated}");

                                Console.ResetColor();




                            }
                            catch (Exception)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($"{DateTime.Now} | Simulator | Failed to publish message");
                                Log.Information($"Simulator | Failed to publish message");

                                //messageQueue.Clear();
                                Console.ResetColor();

                            }

                            //System.Threading.Thread.Sleep(2000);

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;

                            Console.WriteLine($"{DateTime.Now} | Simulator | Connection to RabbitMQ is Closed.  Reconnecting...");
                            Log.Information($"Simulator | Connection to RabbitMQ is Closed.  Reconnecting...");

                            Console.ResetColor();

                        }



                    }

                    Thread.Sleep(_configuration.appSettingModel.PublishRequestTime);

                }


            }
            //}
            //catching 
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




        //public void GenerateDeviceData(PublisherGetting device)
        //{


        //          var factory = new ConnectionFactory { HostName = _AMQPSettings.hostName, UserName = _AMQPSettings.userName, Password = _AMQPSettings.password, Port = _AMQPSettings.port };

        //          while (true)
        //          {
        //              try
        //              {


        //                         DeviceDataModel deviceData = new DeviceDataModel();
        //                         DeviceDataModel deviceDataEmpty = new DeviceDataModel();

        //                         List<Guid> alarmData = device.alarmsList;

        //                         // Create a random number generator
        //                         Random random = new Random();
        //                         int randomIndex = random.Next(0, alarmData.Count - 1);
        //                         Guid randomlySelectedAlarmId = alarmData[randomIndex];


        //                         deviceData.DeviceCode = device.deviceCode;
        //                         deviceData.RandomAlarmId = randomlySelectedAlarmId;


        //                         deviceDataEmpty.DeviceCode = device.deviceCode;
        //                         deviceDataEmpty.RandomAlarmId = Guid.Empty;






        //            using (var connection = factory.CreateConnection())
        //                  using (var channel = connection.CreateModel())
        //                  {

        //                      string exchangeName = _configuration.appSettingModel.ExchangeName;
        //                      string queueName = _configuration.appSettingModel.Queue;

        //                      channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
        //                      channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        //                      channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");





        //                     int tempCount = 0;
        //                     int flag = 0;

        //                    while (true)
        //                    {
        //                      flag = 1;
        //                      tempCount++;

        //                        deviceData.DateCreated = DateTime.Now;
        //                        deviceDataEmpty.DateCreated = DateTime.Now;


        //                        string json = JsonSerializer.Serialize(deviceData);
        //                        string jsonEmpty = JsonSerializer.Serialize(deviceDataEmpty);


        //                    if (_configuration.appSettingModel.PublishRequestTime*tempCount > device.alarmInterval*1000)
        //                      {
        //                        tempCount = 0;
        //                        flag = 1;



        //                        if (connection.IsOpen)
        //                        {

        //                            try
        //                            {

        //                                byte[] body = Encoding.UTF8.GetBytes(json);

        //                                channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);


        //                                Console.ForegroundColor = ConsoleColor.DarkGreen;
        //                                Console.WriteLine($"{deviceData.DeviceCode},{deviceData.RandomAlarmId},{deviceData.DateCreated}");
        //                                Console.ResetColor();

        //                                flag = 1;




        //                            }
        //                            catch (Exception)
        //                            {
        //                                Console.ForegroundColor = ConsoleColor.DarkRed;
        //                                Console.WriteLine($"{DateTime.Now} | Simulator | Failed to publish message");
        //                                Log.Information($"Simulator | Failed to publish message");

        //                                //messageQueue.Clear();
        //                                Console.ResetColor();

        //                            }

        //                            //System.Threading.Thread.Sleep(2000);

        //                        }
        //                        else
        //                        {
        //                            Console.ForegroundColor = ConsoleColor.DarkRed;

        //                            Console.WriteLine($"{DateTime.Now} | Simulator | Connection to RabbitMQ is Closed.  Reconnecting...");
        //                            Log.Information($"Simulator | Connection to RabbitMQ is Closed.  Reconnecting...");

        //                            Console.ResetColor();

        //                        }


        //                      }

        //                     if(flag != 1 )
        //                     {
        //                        if (connection.IsOpen)
        //                        {

        //                            try
        //                            {

        //                                byte[] body = Encoding.UTF8.GetBytes(jsonEmpty);

        //                                channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);


        //                                Console.ForegroundColor = ConsoleColor.DarkGreen;
        //                                Console.WriteLine($"{deviceDataEmpty.DeviceCode},{deviceDataEmpty.RandomAlarmId},{deviceDataEmpty.DateCreated}");

        //                                Console.ResetColor();




        //                            }
        //                            catch (Exception)
        //                            {
        //                                Console.ForegroundColor = ConsoleColor.DarkRed;
        //                                Console.WriteLine($"{DateTime.Now} | Simulator | Failed to publish message");
        //                                Log.Information($"Simulator | Failed to publish message");

        //                                //messageQueue.Clear();
        //                                Console.ResetColor();

        //                            }

        //                            //System.Threading.Thread.Sleep(2000);

        //                        }
        //                        else
        //                        {
        //                            Console.ForegroundColor = ConsoleColor.DarkRed;

        //                            Console.WriteLine($"{DateTime.Now} | Simulator | Connection to RabbitMQ is Closed.  Reconnecting...");
        //                            Log.Information($"Simulator | Connection to RabbitMQ is Closed.  Reconnecting...");

        //                            Console.ResetColor();

        //                        }



        //                    }

        //                          Thread.Sleep(_configuration.appSettingModel.PublishRequestTime);

        //                    }

        //                      break;
        //                  }
        //              }
        //              //catching 
        //              catch (BrokerUnreachableException ex)
        //              {
        //                  Console.ForegroundColor = ConsoleColor.DarkRed;

        //                  Console.WriteLine($"{DateTime.Now} | Simulator | Failed to connect to RabbitMQ: {ex.Message}");
        //                  Log.Information($"Simulator | Failed to connect to RabbitMQ: {ex.Message}");

        //                  Console.ResetColor();
        //              }
        //              catch (Exception ex)
        //              {
        //                  Console.ForegroundColor = ConsoleColor.DarkRed;

        //                  Console.WriteLine($"{DateTime.Now} | Simulator | Failed to connect to RabbitMQ: {ex.Message}");
        //                  Log.Information($" Simulator | Failed to connect to RabbitMQ: {ex.Message}");

        //                  Console.ResetColor();

        //              }

        //               //break;
        //          }









        //}






    }
}

