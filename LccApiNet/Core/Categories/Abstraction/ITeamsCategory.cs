using LccApiNet.Model.General.Enums;
using LccApiNet.Model.Teams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core.Categories.Abstraction
{
    /// <summary>
    /// Contains methods of the /team/ API category. 
    /// </summary>
    public interface ITeamsCategory
    {
        /// <summary>
        /// Creates team and sets its name.
        /// </summary>
        /// <param name="teamName">Name of the team.</param>
        /// <returns>Id of the team.</returns>
        Task<int> CreateTeamAsync(string teamName, CancellationToken token = default);

        /// <summary>
        /// Gets user's teams.
        /// </summary>
        /// <returns>User's teams.</returns>
        Task<Team[]> GetTeamsAsync(CancellationToken token = default);

        /// <summary>
        /// Changes team name.
        /// </summary>
        /// <param name="teamName">Name of the team.</param>
        Task ChangeTeamNameAsync(string teamName, CancellationToken token = default);

        /// <summary>
        /// Deletes team with specific id.
        /// </summary>
        /// <param name="teamId">Id of the team.</param>
        Task DeleteTeamAsync(int teamId, CancellationToken token = default);

        /// <summary>
        /// Adds member in the team.
        /// </summary>
        /// <param name="teamId">Id of the team.</param>
        /// <param name="memberName">Member's name.</param>
        /// <param name="memberRole">Member's role.</param>
        /// <returns>Member's id.</returns>
        Task<int> AddTeamMemberAsync(int teamId, string memberName, Role memberRole,  CancellationToken token = default); 

        /// <summary>
        /// Deletes team member with specific id.
        /// </summary>
        /// <param name="teamId">Id of the team.</param>
        /// <param name="memberId">Member's id</param>
        Task DeleteTeamMemberAsync(int teamId, int memberId, CancellationToken token = default);

        /// <summary>
        /// Changes role of the member of the team.
        /// </summary>
        /// <param name="teamId">Id of the team.</param>
        /// <param name="memberId">If of the member</param>
        /// <param name="role">New role of the member</param>
        Task ChangeMemberRoleAsync(int teamId, int memberId, Role role, CancellationToken token = default);
        
        /// <summary>
        /// Sets new leader of the team
        /// </summary>
        /// <param name="teamId">Id of the team</param>
        /// <param name="newLeaderId">Id of the new leader of the team</param>
        Task ChangeTeamLeaderAsync(int teamId, int newLeaderId, CancellationToken token = default);
    }
}
