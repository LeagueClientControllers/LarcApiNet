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
            bool correctLoginResponse = await _api.AuthorizeDevice(
                "Test",
                ApiCredentials.TEST_ACCOUNT_PASSWORD,
                "TestController",
                DeviceType.Controller);
            
            bool incorrectLoginResponse = await _api.AuthorizeDevice(
                "Test",
                "00000",
                "TestController",
                DeviceType.Controller);

            Assert.True(correctLoginResponse);
            Assert.False(incorrectLoginResponse);

            Assert.True(Assert.CatchAsync(typeof(MethodException), async () => {
                await _api.AuthorizeDevice(
                    "Test",
                    "",
                    "TestController",
                    DeviceType.Controller);
            }) is MethodException exception && exception.ErrorName == MethodError.InvalidMethodParameter);
        }
    }
}
