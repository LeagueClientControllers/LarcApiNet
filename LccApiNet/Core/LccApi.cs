using Ardalis.SmartEnum.JsonNet;
using JWT.Algorithms;
using JWT.Builder;

using LccApiNet.Core.Categories;
using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Core.Services;
using LccApiNet.Exceptions;
using LccApiNet.Model.General;
using LccApiNet.Security;
using LccApiNet.Utilities;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core
{
    /// <summary>
    /// Main API class
    /// </summary>
    public class LccApi : ILccApi
    {
        /// <inheritdoc />
        public JwtPayload? AccessTokenContent { get; private set; }
        
        private const int PORT = 56067;
        private const string API_HOST = "150.230.151.8";
        private Uri _baseUri = new Uri($"http://{API_HOST}:{PORT}");

        /// <inheritdoc />
        public string? AccessToken { get; private set; }

        /// <inheritdoc />
        public IIdentityCategory Identity { get; }

        /// <inheritdoc />
        public IDeviceCategory Device { get; }
        
        /// <inheritdoc />
        public ITeamsCategory Teams { get; }

        private IUserCreditionalsStorage _userCreditionalsStorage = null!;

        
        /// <inheritdoc />
        public ILongPollCategory LongPoll { get; }

        /// <inheritdoc />  
        public IClientCategory Client { get; }

        /// <inheritdoc />
        public UserEventService UserEvents { get; }

        /// <summary>
        /// Creates new instance of the main API class
        /// </summary>
        public LccApi()
        {
            Identity = new IdentityCategory(this);
            Device = new DeviceCategory(this);
            Teams = new TeamsCategory(this);
            LongPoll = new LongPollCategory(this);
            Client = new ClientCategory(this);

            UserEvents = new UserEventService(this);
        }

        /// <inheritdoc />
        public async Task InitAsync(IUserCreditionalsStorage userCreditionalsStorage, CancellationToken token = default)
        {
            string? accessToken = await userCreditionalsStorage.RetrieveAccessTokenAsync().ConfigureAwait(false);
            _userCreditionalsStorage = userCreditionalsStorage;
            if (accessToken == null)
                return;

            AccessToken = accessToken;

            try {
                await Identity.RefreshAccessTokenAsync(token).ConfigureAwait(false);
            } catch (MethodException me) {
                if (me.ErrorName == MethodError.WrongAccessToken) {
                    AccessToken = null;
                    await userCreditionalsStorage.ClearAccessTokenAsync().ConfigureAwait(false);
                }
            }
        }

        /// <inheritdoc />
        public async Task<TResponse> ExecuteAsync<TResponse, TParameters>(string methodPath, TParameters @params, bool withAccessToken = true, CancellationToken token = default)
            where TResponse : ApiResponse
        {
            string payload = JsonConvert.SerializeObject(@params);
            return await ExecuteBase<TResponse>(methodPath, withAccessToken, token, payload).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync<TParameters>(string methodPath, TParameters @params, bool withAccessToken = true, CancellationToken token = default)
        {
            string payload = JsonConvert.SerializeObject(@params);
            await ExecuteBase<ApiResponse>(methodPath, withAccessToken, token, payload).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<TResponse> ExecuteAsync<TResponse>(string methodPath, bool withAccessToken = true, CancellationToken token = default)
            where TResponse : ApiResponse => ExecuteBase<TResponse>(methodPath, withAccessToken, token);

        /// <inheritdoc />
        public async Task UpdateAccessToken(string accessToken, bool storeInSystem = false)
        {
            AccessToken = accessToken;
            if (storeInSystem)
                await _userCreditionalsStorage.StoreAccessTokenAsync(accessToken).ConfigureAwait(false);

            AccessTokenContent = 
                JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(ApiCredentials.JWT_SECRET)
                    .DoNotVerifySignature()
                    .Decode<JwtPayload>(AccessToken);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(string methodPath, bool withAccessToken = true, CancellationToken token = default)
            => ExecuteBase<ApiResponse>(methodPath, withAccessToken, token);

        /// <inheritdoc />
        public async Task<TResponse> ExecuteAsync<TResponse, TParameters>(string methodPath, TParameters @params, string responseObjectKey, bool withAccessToken = true, CancellationToken token = default)
        {
            string payload = JsonConvert.SerializeObject(@params);
            return await ExecuteBase<TResponse>(methodPath, responseObjectKey, withAccessToken, token, payload).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<TResponse> ExecuteAsync<TResponse, TParameter>(string methodPath, string paramKey, TParameter param, bool withAccessToken = true, CancellationToken token = default) 
            where TResponse : ApiResponse
        {
            if (typeof(TParameter).IsPrimitive || typeof(TParameter) == typeof(string) || typeof(TParameter) == typeof(decimal)) { 
                string payload = $"{{ \"{paramKey}\" : \"{param}\" }}";
                return await ExecuteBase<TResponse>(methodPath, withAccessToken, token, payload).ConfigureAwait(false);
            } else {
                throw new WrongSimpleParameterTypeExeption(typeof(TParameter).ToString());
            }
        }

        /// <inheritdoc />
        public async Task<TResponse> ExecuteAsync<TResponse, TParameter>(string methodPath, string paramKey, TParameter param, string responseObjectKey, bool withAccessToken = true, CancellationToken token = default)
        {
            if (typeof(TParameter).IsPrimitive || typeof(TParameter) == typeof(string) || typeof(TParameter) == typeof(decimal)) {
                string payload = $"{{ \"{paramKey}\" : \"{param}\" }}";
                return await ExecuteBase<TResponse>(methodPath, responseObjectKey, withAccessToken, token, payload).ConfigureAwait(false);
            } else {
                throw new WrongSimpleParameterTypeExeption(typeof(TParameter).ToString());
            }
        }

        /// <inheritdoc />
        public Task<TResponse> ExecuteAsync<TResponse>(string methodPath, string responseObjectKey, bool withAccessToken = true, CancellationToken token = default)
            => ExecuteBase<TResponse>(methodPath, responseObjectKey, withAccessToken, token);

        /// <inheritdoc />
        public async Task ExecuteAsync<TParameter>(string methodPath, string paramKey, TParameter param, bool withAccessToken = true, CancellationToken token = default)
        {
            if (typeof(TParameter).IsPrimitive || typeof(TParameter) == typeof(string) || typeof(TParameter) == typeof(decimal)) {
                string payload = $"{{ \"{paramKey}\" : \"{param}\" }}";
                await ExecuteBase<ApiResponse>(methodPath, withAccessToken, token, payload).ConfigureAwait(false);
            } else {
                throw new WrongSimpleParameterTypeExeption(typeof(TParameter).ToString());
            }
        }


        private async Task<TResponse> ExecuteBase<TResponse>(string methodPath, bool withAccessToken, CancellationToken token, string? payload = null)
            where TResponse : ApiResponse
        {
            string responseBody = await _ExecuteBase(methodPath, payload, withAccessToken, token).ConfigureAwait(false);
            TResponse? responseEntity = JsonConvert.DeserializeObject<TResponse>(responseBody);
            Debug.WriteLine($"Executed {methodPath}. Request - {payload},  Response - {{{JsonConvert.SerializeObject(responseEntity, Formatting.Indented)}}} ");

            if (responseEntity == null) { 
                throw new MissingResponseException(methodPath);
            }

            if (responseEntity.Result != ExecutionResult.Error) { 
                return responseEntity;
            }

            if (responseEntity.ErrorName == null || responseEntity.ErrorMessage == null) { 
                throw new WrongResponseException(methodPath);
            }

            throw new MethodException(methodPath, responseEntity.ErrorName, responseEntity.ErrorMessage);
        }


        private async Task<TResponse> ExecuteBase<TResponse>(string methodPath, string responseObjectKey, bool withAccessToken, CancellationToken token, string? payload = null)
        {
            string responseBody = await _ExecuteBase(methodPath, payload, withAccessToken, token).ConfigureAwait(false);

            Type type = CustomApiResponseTypeBuilder.GetCustomApiResponseType<TResponse>(responseObjectKey);
            object? responseEntity = JsonConvert.DeserializeObject(responseBody, type);
            
            if (responseEntity == null) { 
                throw new MissingResponseException(methodPath);
            }

            ApiResponse apiResponse = (ApiResponse)responseEntity;
            
            if (apiResponse.Result != ExecutionResult.Error) {
                PropertyInfo responseObjectProperty = type.GetProperty(string.Concat(responseObjectKey[0].ToString().ToUpper(), responseObjectKey.AsSpan(1)))!;
                TResponse responseObjectPropertyValue = (TResponse)responseObjectProperty.GetValue(responseEntity);
                Console.WriteLine($"[ExecuteBase] TResponse type: {typeof(TResponse)}");

                return responseObjectPropertyValue;
            }

            if (apiResponse.ErrorName == null || apiResponse.ErrorMessage == null) { 
                throw new WrongResponseException(methodPath);
            }

            throw new MethodException(methodPath, apiResponse.ErrorName, apiResponse.ErrorMessage);
        }

        private async Task<string> _ExecuteBase(string methodPath, string? payload, bool withAccessToken, CancellationToken token)
        {
            HttpResponseMessage response;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, methodPath);
            request.Content = new StringContent(payload ?? "", Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _baseUri;
                client.DefaultRequestHeaders.Add("Accept", "application/json;charset=utf-8");
                client.DefaultRequestHeaders.Add("x-api-key", ApiCredentials.API_KEY);

                if (withAccessToken)
                {
                    if (AccessToken == null) throw new LccUserNotAuthorizedException();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                }

                try
                {
                    response = await client.SendAsync(request, token).ConfigureAwait(false);
                }
                catch (HttpRequestException e)
                {
                    response = null;
                    if (response == null)
                    {
                        if ( e.InnerException is SocketException sE && sE.SocketErrorCode == SocketError.ConnectionRefused)
                        {
                            throw new ServerUnreachableException();
                        }

                        throw e;
                    }

                    if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new ApiServerException();
                    }
                }

            }

            string responseString = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
            Debug.WriteLine($"Executed {methodPath}. Request - {payload},  Response - {responseString} ");

            return responseString;    
        }
    }
}