using LccApiNet.Core;
using LccApiNet.Model.Device;
using LccApiNet.Model.Identity.Methods;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
                "dXR)A!^tV93cCKr:;cc\'za*({MMg-R9\"Nm^8`&H]Tg_g)t\"xY<7hW97yr<EL,x,B`eFtX`46V8{TRtXeZXuP%jR<WdpZu$D*EU}KEwHnFN%UV2*^ZMu(\"f%S3`!xuFNj",
                DeviceType.Controller,
                "TestController"
            ), saveCredentials: false);
        }

        [Test]
        public async Task GetDevicesTest() {
            ReadOnlyCollection<Device> devices = await _api.Device.GetDevices();
            
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
