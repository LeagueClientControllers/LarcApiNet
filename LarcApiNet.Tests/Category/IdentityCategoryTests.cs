using LarcApiNet.Exceptions;
using LarcApiNet.Model;

using NUnit.Framework;

using System.Threading.Tasks;

namespace LarcApiNet.Tests.Category
{
    public class IdentityCategoryTests
    {
        private ILarcApi _api = null!;
        
        [SetUp]
        public void Setup() {
            _api = new LarcApi();
        }

        [Test]
        public async Task LoginTest() {
            bool correctLoginResponse = await _api.AuthorizeDevice(
                "Rayms",
                "12345",
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
