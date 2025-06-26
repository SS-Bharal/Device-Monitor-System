using Simulator.Models;
using RabbitMQ.Client;
using API.DataModels;

namespace Simulator.Interfaces
{
    public interface IDataSimulatorService
    {
        public Task GenerateData();

        //public void PublishData();
        public void GenerateDeviceData(PublisherGetting device);

    }
}
