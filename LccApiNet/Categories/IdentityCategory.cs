using LccApiNet.Categories.Abstraction;
using LccApiNet.Exceptions;
using LccApiNet.Model.General;
using LccApiNet.Model.Identity;
using LccApiNet.Model.Identity.Methods;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Categories
{
    /// <summary>
    /// API category that contains methods related to user identification
    /// </summary>
    class IdentityCategory : IIdentityCategory
    {
        private ILccApi _api;

        public IdentityCategory(ILccApi api)
        {
            _api = api;
        }


        /// <inheritdoc />
        public async Task<bool> LoginAsync(LoginParameters @params, bool saveCredentials, CancellationToken token = default)
        {
            try {
                string response = await _api.ExecuteAsync<string, LoginParameters>("/identity/login", @params, "accessToken", false, token).ConfigureAwait(false);
                if (response == null)
                    throw new WrongResponseException("/identity/login");

                await _api.UpdateAccessToken(response, saveCredentials).ConfigureAwait(false);
                return true;
            } catch (MethodException me) {
                if (me.ErrorName == MethodError.WrongNicknameEmailOrPassword || me.ErrorName == MethodError.InvalidMethodParameter) {
                    return false;
                } else {
                    throw me;
                }
            }
        }

        /// <inheritdoc />
        public async Task RefreshAccessTokenAsync(CancellationToken token = default)
        {
            string? response = await _api.ExecuteAsync<string?>("/identity/refreshAccessToken", "accessToken", true, token).ConfigureAwait(false);
            if (response == null)
                throw new WrongResponseException("/identity/login");

            await _api.UpdateAccessToken(response).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<ProfileInfo> GetProfileInfoAsync(CancellationToken token = default)
        {
            ProfileInfoResponse response = await _api.ExecuteAsync<ProfileInfoResponse>("/identity/getProfileInfo", true, token).ConfigureAwait(false);

            return response.Profile;
        }
    }
}
