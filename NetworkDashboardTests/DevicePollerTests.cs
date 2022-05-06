using Microsoft.VisualStudio.TestTools.UnitTesting;
using NDDevicePoller.Models;
using NDDevicePoller;
using NetworkDashboard;


namespace NDDevicePollerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PingDeviceTest()
        {
            bool expected = true;
            //This will require a device to test against.
            NetworkDevice netDevice = new NetworkDevice()
            {
                Id = 1,
                DeviceName = "Router-1",
                DisplayName = "Home Gateway Router",
                DeviceDescription = "Gateway Router",
                DeviceType = "Router",
                IPAddress = "192.168.0.1",
                IsConnected = false,
            };
            Worker _worker = new Worker(null);

            netDevice.IsConnected = true; // this needs to reference the PingHost Method

            Assert.IsTrue(netDevice.IsConnected);

        }
    }
}