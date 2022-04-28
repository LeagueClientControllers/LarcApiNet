using LccApiNet.Model;

using NUnit.Framework;

using System.Threading.Tasks;

namespace LccApiNet.Tests.Category
{
    public class TeamsCategoryTest
    {
        private ILccApi _api = null!;

        [SetUp]
        public async Task Setup()
        {
            _api = new LccApi();
            await _api.AuthorizeDevice(
                "Rayms",
                "12345",
                "TestController",
                DeviceType.Controller);
        }

        //[Test]
        //public async Task AddTeamMemberTest()
        //{
        //    int correctAddMemberResponse = await _api.Teams.AddTeamMemberAsync(20, "06piKi--LaNbJ4W_Llmq0QC3WBbQBcPcresn6vBOrLpD7zo", Role.Bot).ConfigureAwait(false);

        //    Assert.CatchAsync(typeof(ApiServerException), async () => {
        //        int teamId = await _api.Teams.AddTeamMemberAsync(20, "", Role.Bot).ConfigureAwait(false);
        //    }, "It is not possible to add a player without [SummonerId]");

        //    Assert.CatchAsync(typeof(ApiServerException), async () => {
        //        int teamId = await _api.Teams.AddTeamMemberAsync(21, "06piKi--LaNbJ4W_Llmq0QC3WBbQBcPcresn6vBOrLpD7zo", Role.Bot).ConfigureAwait(false);
        //    }, "Team doesn't exist");

        //    Assert.AreEqual(16, correctAddMemberResponse);
        //}
    }
}
