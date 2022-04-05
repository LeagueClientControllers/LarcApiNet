using LccApiNet.Model;

using NUnit.Framework;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LccApiNet.Tests.Category
{
    public class DeviceCategoryTests
    {
        private ILccApi _api = null!;

        [SetUp]
        public async Task Setup()
        {
            _api = new LccApi();
            await _api.Identity.LoginAsync(new LoginParameters(
                "Test",
                ApiCredentials.TEST_ACCOUNT_PASSWORD,
                "TestController",
                DeviceType.Controller
            ), saveCredentials: false);
        }

        [Test]
        public async Task GetDevicesTest() {
            List<Device> devices = await _api.Device.GetDevicesAsync();
            
            Assert.AreEqual(2, devices.Count);
            
            Assert.AreEqual(devices[0].Id, 20);
            Assert.AreEqual(devices[0].Type, DeviceType.Phone);
            Assert.AreEqual(devices[0].Name, "iPhone 12");
            Assert.AreEqual(devices[0].IsOnline, false);

            Assert.AreEqual(devices[1].Id, 21);
            Assert.AreEqual(devices[1].Type, DeviceType.Phone);
            Assert.AreEqual(devices[1].Name, "Samsung Galaxy S10");
            Assert.AreEqual(devices[1].IsOnline, true);
        }
    }
}
