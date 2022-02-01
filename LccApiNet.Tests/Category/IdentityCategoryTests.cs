using LccApiNet.Model.Device;
using LccApiNet.Model.Identity.Methods;

using NUnit.Framework;

using System.Threading.Tasks;

namespace LccApiNet.Tests.Category
{
    public class IdentityCategoryTests
    {
        private ILccApi _api = null!;
        
        [SetUp]
        public void Setup() {
            _api = new LccApi();
        }

        [Test]
        public async Task LoginTest() {

            bool correctLoginResponse = await _api.Identity.LoginAsync(new LoginParameters(
                "Test", 
                "dXR)A!^tV93cCKr:;cc\'za*({MMg-R9\"Nm^8`&H]Tg_g)t\"xY<7hW97yr<EL,x,B`eFtX`46V8{TRtXeZXuP%jR<WdpZu$D*EU}KEwHnFN%UV2*^ZMu(\"f%S3`!xuFNj",
                DeviceType.Controller,
                "TestController"
            ), saveCredentials: false);
            
            bool incorrectLoginResponse = await _api.Identity.LoginAsync(new LoginParameters(
                "Test",
                "none",
                DeviceType.Controller,
                "TestController"
            ), saveCredentials: false);

            bool incorrectSecondLoginResponse = await _api.Identity.LoginAsync(new LoginParameters(
                "Test",
                "",
                DeviceType.Controller,
                "TestController"
            ), saveCredentials: false);

            Assert.True(correctLoginResponse);
            Assert.False(incorrectLoginResponse);
            Assert.False(incorrectSecondLoginResponse);
        }
    }
}
