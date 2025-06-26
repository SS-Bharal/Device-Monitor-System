using API.DataModels;

namespace API.Interfaces
{
    public interface IMtService
    {
        public Task<ServiceResponseModel> GetDevicesDataWithAlarms();
        public Task<ServiceResponseModel> GetAMQPSettings();
        public Task<ServiceResponseModel> GetAlarmList();
        public Task<ServiceResponseModel> GetChartDataFromDeviceData(string devicecode, DateTime date);
        public Task<ServiceResponseModel> GetTableDataFromDeviceData(string devicecode, DateTime date);
        public Task<ServiceResponseModel> PostDeviceCreated(DeviceInsertionModel data);
        public Task<ServiceResponseModel> PostDeviceData(DeviceDataModel data);
    }
}
