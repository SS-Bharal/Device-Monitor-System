using API.DataModels;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MtController : Controller
    {
        private readonly IMtService _service;
        public MtController(IMtService service)
        {
            _service = service;
        }

        #region GET APIs

        // GET API That Get Devices Including Alarms For Simulator
        [HttpGet("getsimulatordata")]
        public async Task<ObjectResult> DevicesDataWithAlarms()
        {

            var result = await _service.GetDevicesDataWithAlarms();
            return StatusCode(result.StatusCode, result);

        }

        // GET API That Get RabbitMQ configuration from AMQPSettings
        [HttpGet("getamqpsettings")]
        public async Task<ObjectResult> AMQPSettings()
        {

            var result = await _service.GetAMQPSettings();
            return StatusCode(result.StatusCode, result);

        }

        // GET API That Get RabbitMQ configuration from Alarm List
        [HttpGet("getalarmlist")]
        public async Task<ObjectResult> AlarmList()
        {

            var result = await _service.GetAlarmList();
            return StatusCode(result.StatusCode, result);

        }


        // GET API That Provide Device Alarm Report as per UI Dashboard.
        [HttpGet("getchartdata/{devicecode}/{date}")]
        public async Task<ObjectResult> ChartDataFromDeviceData([FromRoute] string devicecode, [FromRoute] DateTime date)
        {

            var result = await _service.GetChartDataFromDeviceData(devicecode, date);
            return StatusCode(result.StatusCode, result);

        }

        // GET API That Provide Device Alarm Report as per UI Dashboard.
        [HttpGet("gettabletdata/{devicecode}/{date}")]
        public async Task<ObjectResult> TableDataFromDeviceData([FromRoute] string devicecode, [FromRoute] DateTime date)
        {

            var result = await _service.GetTableDataFromDeviceData(devicecode, date);
            return StatusCode(result.StatusCode, result);

        }




        #endregion

        #region POST APIs



        // POST API to Register Devices.
        [HttpPost("createdevice")]
        public async Task<ObjectResult> DeviceCreated([FromBody] DeviceInsertionModel Device)
        {

            var result = await _service.PostDeviceCreated(Device);
            return StatusCode(result.StatusCode, result);

        }

        // POST API Service to Save Data DeviceData.
        [HttpPost("savedevice")]
        public async Task<ObjectResult> ChartDataFromDeviceData([FromBody] DeviceDataModel Device)
        {

            var result = await _service.PostDeviceData(Device);
            return StatusCode(result.StatusCode, result);

        }








            #endregion

    }
}
