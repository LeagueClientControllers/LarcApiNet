using LccApiNet.Model.General.Enums;
using LccApiNet.Model.Teams;
using LccApiNet.Model.Teams.Methods;
using System;
using System.Threading;
using System.Threading.Tasks;
using LccApiNet.Core.Categories.Abstraction;

namespace LccApiNet.Core.Categories
{
    public class TeamsCategory : ITeamsCategory
    {
        private ILccApi _api;

        public TeamsCategory(ILccApi api) {
            _api = api;
        }

        /// <inheritdoc />
        public async Task<int> CreateTeamAsync(string teamName, CancellationToken token = default)
        {
            if (teamName == "") 
                throw new ArgumentException("Team name cannot be empty");

            CreateTeamResponse response = await _api.ExecuteAsync<CreateTeamResponse, CreateTeamParameters>("/teams/create", new CreateTeamParameters(teamName), true, token).ConfigureAwait(false);
            
            return response.TeamId;
        }

        /// <inheritdoc />
        public async Task<Team[]> GetTeamsAsync(CancellationToken token = default)
        {
            TeamsResponse response = await _api.ExecuteAsync<TeamsResponse>("/teams/getTeams", true, token).ConfigureAwait(false);

            return response.Teams;
        }

        /// <inheritdoc />
        public async Task ChangeTeamNameAsync(string teamName, CancellationToken token = default)
        {

            if (teamName == "") 
                throw new ArgumentException("Team name cannot be empty");

            await _api.ExecuteAsync("/teams/changeName", new CreateTeamParameters(teamName), true, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteTeamAsync(int teamId, CancellationToken token = default)
        {
            await _api.ExecuteAsync("/teams/delete", new DeleteTeamParameters(teamId), true, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<int> AddTeamMemberAsync(int teamId, string memberSummonerId, Role memberRole, CancellationToken token = default)
        {
            AddTeamMemberParameters @params = new AddTeamMemberParameters(teamId, memberSummonerId, memberRole);

            AddTeamMemberResponse response = 
                await _api.ExecuteAsync<AddTeamMemberResponse, AddTeamMemberParameters>(
                    "/teams/addMember", @params, true, token).ConfigureAwait(false);

            return response.MemberId;
        }

        /// <inheritdoc />
        public async Task DeleteTeamMemberAsync(int teamId, int memberId, CancellationToken token = default)
        {
            if (_api.AccessTokenContent!.UserId == memberId)
                throw new ArgumentException("User cannot delete himself from the team");

            await _api.ExecuteAsync("teams/deleteMember", new DeleteTeamMemberParameters(teamId, memberId), true, token).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task ChangeMemberRole(int teamId, int memberId, Role role, CancellationToken token = default) {
            await _api.ExecuteAsync("teams/changeMemberRole", new ChangeMemberRoleParameters(teamId, memberId, role), true, token).ConfigureAwait(false);    
        }
    }
}
