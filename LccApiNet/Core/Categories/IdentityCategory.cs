using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Enums.Safety;
using LccApiNet.Exceptions;
using LccApiNet.Model.Identity.Methods;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core.Categories
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
                AccessTokenResponse response = await _api.ExecuteAsync<AccessTokenResponse, LoginParameters>("/identity/login", @params, false, token).ConfigureAwait(false);
                if (response.AccessToken == null)
                    throw new WrongResponseException("/identity/login");

                await _api.UpdateAccessToken(response.AccessToken, saveCredentials).ConfigureAwait(false);
                return true;
            } catch (MethodException me) {
                if (me.ErrorName == MethodError.WrongNicknameEmailOrPassword || me.ErrorName == MethodError.InvalidMethodParameter) {
                    return false;
                } else {
                    throw me;
                }
            } catch (Exception e) {
                throw e;
            }
        }

        /// <inheritdoc />
        public async Task RefreshAccessTokenAsync(CancellationToken token = default)
        {
            AccessTokenResponse response = await _api.ExecuteAsync<AccessTokenResponse>("/identity/refreshAccessToken", true, token).ConfigureAwait(false);
            if (response.AccessToken == null)
                throw new WrongResponseException("/identity/login");

            await _api.UpdateAccessToken(response.AccessToken).ConfigureAwait(false);
        }
    }
}
