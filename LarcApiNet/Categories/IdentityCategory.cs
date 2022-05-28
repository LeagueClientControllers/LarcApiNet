#nullable enable
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LarcApiNet;
using LarcApiNet.Categories.Abstraction;
using LarcApiNet.Exceptions;
using LarcApiNet.Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


namespace LarcApiNet.Categories {
    
    
    /// <inheritdoc />
    public class IdentityCategory : IIdentityCategory {
        
        private ILarcApi _api;
        
        public IdentityCategory(ILarcApi api) {
            _api = api;
        }
        
        /// <inheritdoc />
        public async Task<int> RegisterAsync(string username, string email, string password, CancellationToken token = default) 
        {
        	// <auto-generated-safe-area> Code within tag borders shouldn't cause incorrect behavior and will be preserved.
        	// TODO: Add parameters validation
        	// </auto-generated-safe-area>
        	RegisterParameters parameters = new RegisterParameters (username, email, password);
        	RegistrationResponse response = await _api.ExecuteAsync<RegistrationResponse, RegisterParameters> ("/identity/register", parameters, false, token);
        	// <auto-generated-safe-area> Code within tag borders shouldn't cause incorrect behavior and will be preserved.
        	// TODO: Add response validation
        	// </auto-generated-safe-area>
        	return response.RegisteredId;
        }
        
        /// <inheritdoc />
        public async Task<string> ConfirmAsync(int accountId, string code, string deviceName, DeviceType deviceType, CancellationToken token = default) 
        {
        	// <auto-generated-safe-area> Code within tag borders shouldn't cause incorrect behavior and will be preserved.
        	// TODO: Add parameters validation
        	// </auto-generated-safe-area>
        	ConfirmationParameters parameters = new ConfirmationParameters (accountId, code, deviceName, deviceType);
        	AccessTokenResponse response = await _api.ExecuteAsync<AccessTokenResponse, ConfirmationParameters> ("/identity/confirm", parameters, false, token);
        	// <auto-generated-safe-area> Code within tag borders shouldn't cause incorrect behavior and will be preserved.
        	// TODO: Add response validation
        	// </auto-generated-safe-area>
        	return response.AccessToken;
        }
        
        /// <inheritdoc />
        public async Task ResendConfirmationAsync(int accountId, CancellationToken token = default) 
        {
        	// <auto-generated-safe-area> Code within tag borders shouldn't cause incorrect behavior and will be preserved.
        	// TODO: Add parameters validation
        	// </auto-generated-safe-area>
        	ResendConfirmationParameters parameters = new ResendConfirmationParameters (accountId);
        	await _api.ExecuteAsync<ResendConfirmationParameters> ("/identity/resendConfirmation", parameters, false, token);
        }
        
        /// <inheritdoc />
        public async Task<string> LoginAsync(string login, string password, string deviceName, DeviceType deviceType, CancellationToken token = default) 
        {
        	// <auto-generated-safe-area> Code within tag borders shouldn't cause incorrect behavior and will be preserved.
        	// TODO: Add parameters validation
        	// </auto-generated-safe-area>
        	LoginParameters parameters = new LoginParameters (login, password, deviceName, deviceType);
        	AccessTokenResponse response = await this._api.ExecuteAsync<AccessTokenResponse, LoginParameters> ("/identity/login", parameters, false, token);
        	// <auto-generated-safe-area> Code within tag borders shouldn't cause incorrect behavior and will be preserved.
        	// TODO: Add response validation
        	// </auto-generated-safe-area>
        	return response.AccessToken;
        }
        
        /// <inheritdoc />
        public async Task<ProfileInfo> GetProfileInfoAsync(CancellationToken token = default) 
        {
        	ProfileInfoResponse response = await _api.ExecuteAsync<ProfileInfoResponse> ("/identity/getProfileInfo", true, token).ConfigureAwait (false);
        	return response.Profile;
        }
        
        /// <inheritdoc />
        public async Task<string> RefreshAccessTokenAsync(CancellationToken token = default) 
        {
        	AccessTokenResponse response = await this._api.ExecuteAsync<AccessTokenResponse> ("/identity/refreshAccessToken", true, token);
        	// <auto-generated-safe-area> Code within tag borders shouldn't cause incorrect behavior and will be preserved.
        	// TODO: Add response validation
        	// </auto-generated-safe-area>
        	return response.AccessToken;
        }
    }
}

#nullable restore
