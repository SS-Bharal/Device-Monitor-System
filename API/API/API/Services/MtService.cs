using API.DataModels;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;

namespace API.Services
{
    public class MtService : IMtService
    {
        private readonly DataDbContext _dbContext;
        public MtService(DataDbContext dbContext) {
            _dbContext = dbContext;

            //Error logging
            Log.Logger = new LoggerConfiguration()
              //.WriteTo.Console()
              .WriteTo.File("./Logging/log.txt")
              .CreateLogger();
        }


        #region  GET API Services

        // GET API That Get Devices Including Alarms For Simulator
        public async Task<ServiceResponseModel> GetDevicesDataWithAlarms()
        {
            ServiceResponseModel responseModel = new ServiceResponseModel();
            try
            {

                List<PublisherGetting> queryResult = await _dbContext.Devices
                .Select(device => new PublisherGetting
                {
                    DeviceCode = device.DeviceCode,
                    AlarmInterval = device.AlarmInterval,
                    AlarmsList = device.AlarmDeviceWise.Select(adw => adw.AlarmId).ToList()
                })
                .ToListAsync();

             
                    responseModel.Status = true;
                    responseModel.SuccessMessage = "Chart Data Get Success";
                    responseModel.Result = queryResult;

            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.ErrorMessage = ex.Message;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"API | MtService | {DateTime.Now} | {ex.Message}");
                Log.Information($"API | MtService | {DateTime.Now} | {ex.Message}");
                Console.ResetColor();
            }

            return responseModel;

        }



        // GET API That Get RabbitMQ configuration from AMQPSettings
        public async Task<ServiceResponseModel> GetAMQPSettings()
        {
            ServiceResponseModel responseModel = new ServiceResponseModel();
            try
            {

                AMQPSettingsModel RabbitMqConfig = await _dbContext.AMQPSettings.FirstOrDefaultAsync();

                    responseModel.Status = true;
                    responseModel.SuccessMessage = "Chart Data Get Success";
                    responseModel.Result = RabbitMqConfig;

            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.ErrorMessage = ex.Message;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"API | MtService | {DateTime.Now} | {ex.Message}");
                Log.Information($"API | MtService | {DateTime.Now} | {ex.Message}");
                Console.ResetColor();
            }

            return responseModel;

        }

        // GET API That Get RabbitMQ configuration from Alarm List
        public async Task<ServiceResponseModel> GetAlarmList()
        {
            ServiceResponseModel responseModel = new ServiceResponseModel();
            try
            {

                List<AlarmModel> AlarmList = await _dbContext.Alarms.Where(x=>true).ToListAsync();

                responseModel.Status = true;
                responseModel.SuccessMessage = "Chart Data Get Success";
                responseModel.Result = AlarmList;

            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.ErrorMessage = ex.Message;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"API | MtService | {DateTime.Now} | {ex.Message}");
                Log.Information($"API | MtService | {DateTime.Now} | {ex.Message}");
                Console.ResetColor();
            }

            return responseModel;

        }


        // GET API That Provide Device Alarm Report as per UI Dashboard. ( Chart Data )
        public async Task<ServiceResponseModel> GetChartDataFromDeviceData(string devicecode, DateTime date)
        {
            ServiceResponseModel responseModel = new ServiceResponseModel();
            try
            {

                if (devicecode == null || date == null)
                {
                    responseModel.Status = false;
                    responseModel.ErrorMessage = "device name or interval can not be null";

                }
                else
                {
                    //DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);

                    //List<DeviceDataModel> TableData = _dbContext.DeviceData.Where(x => (x.DeviceCode == devicecode && x.DateCreated.Date == date.Date)).ToList();

                    //string deviceName = await _dbContext.Devices.Where(x => x.DeviceCode == devicecode).Select(x => x.DeviceName).FirstOrDefaultAsync();

                    //List<AlarmModel> deviceAlarmNames = new List<AlarmModel>();
                    //deviceAlarmNames = await _dbContext.Alarms.ToListAsync();

                    ////List<DeviceChartGetting> deviceChartGettings = new List<DeviceChartGetting>();
                    //DeviceChartGetting deviceChartGettings = new DeviceChartGetting();


                    //foreach (var data in deviceAlarmNames)
                    //{

                    //    deviceChartGettings.DeviceName = deviceName;
                    //    deviceChartGettings.DeviceCode = devicecode;
                    //    deviceChartGettings.AlarmName = data.AlarmName;
                    //    deviceChartGettings.AlarmCount = TableData.Where(x=>x.RandomAlarmId == data.AlarmId).Count();

                    //}


                    //--------------------------

                    List<DeviceDataModel> TableData = _dbContext.DeviceData.Where(x => (x.DeviceCode == devicecode && x.DateCreated.Date == date.Date)).ToList();
                    //List<DeviceDataModel> TableData = _dbContext.DeviceData.Where(x => (x.DeviceCode == devicecode &&  && x.DateCreated.Date == date.ToDateTimeUnsafely().Date)).ToList();
                    //List<DeviceDataModel> TableData =await _dbContext.DeviceData.Where(x => true).ToListAsync();



                    string deviceName = await _dbContext.Devices.Where(x => x.DeviceCode == devicecode).Select(x => x.DeviceName).FirstOrDefaultAsync();

                    List<AlarmModel> deviceAlarmNames = new List<AlarmModel>();
                    deviceAlarmNames = await _dbContext.Alarms.ToListAsync();

                    List<DeviceGetting> deviceGettings = new List<DeviceGetting>();

                    foreach (var data in TableData)
                    {
                        DeviceGetting deviceGetting = new DeviceGetting();
                        deviceGetting.DeviceName = deviceName;
                        deviceGetting.DeviceCode = data.DeviceCode;
                        deviceGetting.DateTime = data.DateCreated;
                        deviceGetting.AlarmName = deviceAlarmNames.Where(x => x.AlarmId == data.RandomAlarmId).Select(x => x.AlarmName).FirstOrDefault();
                        deviceGettings.Add(deviceGetting);

                    }






                  
                     Dictionary<string, int> alarmCounts = deviceGettings
                    .Where(data => data.DeviceCode == devicecode && data.AlarmName != null)
                    .GroupBy(data => data.AlarmName)
                    .ToDictionary(group => group.Key, group => group.Count());
                  
                  

                    responseModel.Status = true;
                    responseModel.SuccessMessage = "Chart Data Get Success";
                    responseModel.Result = alarmCounts;

                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.ErrorMessage = ex.Message;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"API | MtService | {DateTime.Now} | {ex.Message}");
                Log.Information($"API | MtService | {DateTime.Now} | {ex.Message}");
                Console.ResetColor();
            }

            return responseModel;


        }


        // GET API That Provide Device Alarm Report as per UI Dashboard (Table Data )
        public async Task<ServiceResponseModel> GetTableDataFromDeviceData(string devicecode, DateTime date)
        {
            ServiceResponseModel responseModel = new ServiceResponseModel();
            try
            {

                if (devicecode == null || date == null)
                {
                    responseModel.Status = false;
                    responseModel.ErrorMessage = "device name or interval can not be null";

                }
                else
                {
                    //DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);

                    List<DeviceDataModel> TableData = _dbContext.DeviceData.Where(x => (x.DeviceCode == devicecode && x.DateCreated.Date == date.Date)).ToList();
                    //List<DeviceDataModel> TableData = _dbContext.DeviceData.Where(x => (x.DeviceCode == devicecode &&  && x.DateCreated.Date == date.ToDateTimeUnsafely().Date)).ToList();
                    //List<DeviceDataModel> TableData =await _dbContext.DeviceData.Where(x => true).ToListAsync();

                  

                    string deviceName = await _dbContext.Devices.Where(x => x.DeviceCode == devicecode).Select(x => x.DeviceName).FirstOrDefaultAsync();

                    List<AlarmModel> deviceAlarmNames = new List<AlarmModel>();
                    deviceAlarmNames = await _dbContext.Alarms.ToListAsync();

                    List<DeviceGetting> deviceGettings = new List<DeviceGetting>();

                    foreach (var data in TableData)
                    {
                        if(data.RandomAlarmId != Guid.Empty)
                        {
                            DeviceGetting deviceGetting = new DeviceGetting();
                            deviceGetting.DeviceName = deviceName;
                            deviceGetting.DeviceCode = data.DeviceCode;
                            deviceGetting.DateTime = data.DateCreated;
                            deviceGetting.AlarmName = deviceAlarmNames.Where(x => x.AlarmId == data.RandomAlarmId).Select(x => x.AlarmName).FirstOrDefault();
                            deviceGettings.Add(deviceGetting);

                        }
                       

                    }

                    responseModel.Status = true;
                    responseModel.SuccessMessage = "Chart Data Get Success";
                    responseModel.Result = deviceGettings;

                }
            }
            catch (Exception ex)
            {
                responseModel.Status = false;
                responseModel.ErrorMessage = ex.Message;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"API | MtService | {DateTime.Now} | {ex.Message}");
                Log.Information($"API | MtService | {DateTime.Now} | {ex.Message}");
                Console.ResetColor();
            }

            return responseModel;


        }




        #endregion


        #region POST API Services

        // POST API Service to Register Devices.
        public async Task<ServiceResponseModel> PostDeviceCreated(DeviceInsertionModel data)
        {
            ServiceResponseModel responseModel = new ServiceResponseModel();
            try
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "Model cannot be null");
                }

                DeviceModel deviceModel = new DeviceModel();
                deviceModel.DeviceName = data.DeviceName;
                deviceModel.DeviceCode = data.DeviceCode;
                deviceModel.AlarmInterval = data.AlarmInterval;
                deviceModel.CreatedOn = DateTime.Now;

                AlarmDeviceWiseModel alarmDevice = new AlarmDeviceWiseModel();

                var records = data.AlarmId.Select(item => new AlarmDeviceWiseModel
                {
                    DeviceCode = data.DeviceCode,
                    AlarmId = item,

                }).ToList();


                //foreach (var alarmId in data.AlarmId)
                //{

                //    alarmDevice.DeviceCode = data.DeviceCode;
                //    alarmDevice.AlarmId = alarmId;
                //    _dbContext.AlarmDeviceWise.Add(alarmDevice);

                //}

                _dbContext.AlarmDeviceWise.AddRange(records);


                _dbContext.Devices.Add(deviceModel);
               

                await _dbContext.SaveChangesAsync();

                responseModel.Status = true;
                responseModel.SuccessMessage = "Data Added Successfully";
                
            }
            catch (ArgumentNullException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine($" API | {ex.Message}");
                Log.Information($" API | {ex.Message}");

                Console.ResetColor();
                
                responseModel.Status= false;
                responseModel.ErrorMessage = "Data Not Added";
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine($" API | {ex.Message}");
                Log.Information($" API | {ex.Message}");

                Console.ResetColor();

                responseModel.Status = false;
                responseModel.ErrorMessage = "Data Not Added";
            }



            return responseModel;

        }


        // POST API Service to Save Data DeviceData.
        public async Task<ServiceResponseModel> PostDeviceData(DeviceDataModel data)
        {
            ServiceResponseModel responseModel = new ServiceResponseModel();
            try
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "Model cannot be null");
                }

                DeviceDataModel deviceModel = new DeviceDataModel();
                deviceModel.DeviceCode = data.DeviceCode;
                deviceModel.DateCreated = DateTime.Now;
                deviceModel.RandomAlarmId = data.RandomAlarmId;


                _dbContext.DeviceData.Add(deviceModel);

                await _dbContext.SaveChangesAsync();

                responseModel.Status = true;
                responseModel.SuccessMessage = "Data Added Successfully";

            }
            catch (ArgumentNullException ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine($" API | {ex.Message}");
                Log.Information($" API | {ex.Message}");

                Console.ResetColor();

                responseModel.Status = false;
                responseModel.ErrorMessage = "Data Not Added";
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine($" API | {ex.Message}");
                Log.Information($" API | {ex.Message}");

                Console.ResetColor();

                responseModel.Status = false;
                responseModel.ErrorMessage = "Data Not Added";
            }



            return responseModel;

        }



        #endregion

    }
}
