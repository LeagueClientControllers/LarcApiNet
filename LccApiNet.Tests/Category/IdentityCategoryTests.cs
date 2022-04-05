using LccApiNet.Exceptions;
using LccApiNet.Model;

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
                ApiCredentials.TEST_ACCOUNT_PASSWORD,
                "TestController",
                DeviceType.Controller
            ), saveCredentials: false);
            
            bool incorrectLoginResponse = await _api.Identity.LoginAsync(new LoginParameters(
                "Test",
                "00000",
                "TestController",
                DeviceType.Controller
            ), saveCredentials: false);

            
            Assert.True(correctLoginResponse);
            Assert.False(incorrectLoginResponse);
            
            MethodException? exception = Assert.CatchAsync(typeof(MethodException), async () => {
                bool incorrectSecondLoginResponse = await _api.Identity.LoginAsync(new LoginParameters(
                    "Test",
                    "",
                    "TestController",
                    DeviceType.Controller
                ), saveCredentials: false);
            }) as MethodException;

            Assert.True(exception != null && exception.ErrorName == MethodError.InvalidMethodParameter);
        }
    }
}
