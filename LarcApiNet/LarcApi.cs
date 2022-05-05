using JWT.Algorithms;
using JWT.Builder;

using LarcApiNet.Categories;
using LarcApiNet.Categories.Abstraction;
using LarcApiNet.Exceptions;
using LarcApiNet.Model;
using LarcApiNet.Security;
using LarcApiNet.Services;
using LarcApiNet.Utilities;

using Newtonsoft.Json;

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using LccApiNet.Security;

namespace LarcApiNet
{
    /// <summary>
    /// Main API class
    /// </summary>
    public class LarcApi : ILarcApi
    {
        /// <inheritdoc />
        public JwtPayload? AccessTokenContent { get; private set; }

        private readonly string _baseUri = $"http://{ILarcApi.API_HOST}";
        private IUserCredentialsStorage _userCredentialsStorage = null!;

        /// <inheritdoc />
        public string? AccessToken { get; private set; }

        #region <auto-generated> Generated code for categories implementation

        /// <inheritdoc />
        public IClientCategory Client { get; }

        /// <inheritdoc />
        public IDeviceCategory Device { get; }

        /// <inheritdoc />
        public IIdentityCategory Identity { get; }

        #endregion

        /// <inheritdoc />
        public EventService Events { get; }

        /// <inheritdoc />
        public CommandService Commands { get; }

        /// <summary>
        /// Creates new instance of the main API class
        /// </summary>
        public LarcApi() {
            #region <auto-generated> Generated code for categories initialization
            Client = new ClientCategory(this);
            Device = new DeviceCategory(this);
            Identity = new IdentityCategory(this);
            #endregion

            Events = new EventService(this);
            Commands = new CommandService(this);
        }

        /// <inheritdoc />
        public async Task InitAsync(IUserCredentialsStorage userCredentialsStorage, CancellationToken token = default)
        {
            _userCredentialsStorage = userCredentialsStorage;
            string? accessToken = await _userCredentialsStorage.RetrieveAccessTokenAsync(token).ConfigureAwait(false);
            if (accessToken == null)
                return;

            AccessToken = accessToken;

            try {
                string freshAccessToken = await Identity.RefreshAccessTokenAsync(token).ConfigureAwait(false);
                await UpdateAccessToken(freshAccessToken, true).ConfigureAwait(false);
            } catch (MethodException me) {
                if (me.ErrorName == MethodError.WrongAccessToken) {
                    AccessToken = null;
                    await _userCredentialsStorage.ClearAccessTokenAsync(token).ConfigureAwait(false);
                }
            }
        }

        public async Task<bool> AuthorizeDevice(string login, string password, string deviceName, DeviceType deviceType,
            bool saveCredentials = false, CancellationToken token = default)
        {
            try {
                string accessToken = await Identity.LoginAsync(login, password, deviceName, deviceType, token);
                await UpdateAccessToken(accessToken, saveCredentials).ConfigureAwait(false);
        		return true;
        	} catch (MethodException me) {
                if (me.ErrorName == MethodError.WrongNicknameEmailOrPassword) {
        			return false;
        		}

                throw;
            }
        }
        
        private async Task UpdateAccessToken(string accessToken, bool storeInSystem = false)
        {
            AccessToken = accessToken;
            if (storeInSystem)
                await _userCredentialsStorage.StoreAccessTokenAsync(accessToken).ConfigureAwait(false);

            AccessTokenContent =
                JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(ApiCredentials.JWT_SECRET)
                    .DoNotVerifySignature()
                    .Decode<JwtPayload>(AccessToken);
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
            where TResponse : ApiResponse
        {
            return ExecuteBase<TResponse>(methodPath, withAccessToken, token);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(string methodPath, bool withAccessToken = true, CancellationToken token = default)
        {
            return ExecuteBase<ApiResponse>(methodPath, withAccessToken, token);
        }

        /// <inheritdoc />
        public async Task<TResponse> ExecuteAsync<TResponse, TParameters>(string methodPath, TParameters @params, string responseObjectKey, bool withAccessToken = true, CancellationToken token = default)
        {
            string payload = JsonConvert.SerializeObject(@params);
            return await ExecuteBase<TResponse>(methodPath, responseObjectKey, withAccessToken, token, payload).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public Task<TResponse> ExecuteAsync<TResponse>(string methodPath, string responseObjectKey, bool withAccessToken = true, CancellationToken token = default)
        {
            return ExecuteBase<TResponse>(methodPath, responseObjectKey, withAccessToken, token);
        }

        private async Task<TResponse> ExecuteBase<TResponse>(string methodPath, bool withAccessToken, CancellationToken token, string? payload = null)
            where TResponse : ApiResponse
        {
            string responseBody = await _ExecuteBase(methodPath, payload, withAccessToken, token).ConfigureAwait(false);

            TResponse? responseEntity;
            try {
                responseEntity = JsonConvert.DeserializeObject<TResponse>(responseBody);
            } catch (JsonSerializationException) {
                throw new WrongResponseException(methodPath);
            }

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
                TResponse? responseObjectPropertyValue = (TResponse?)responseObjectProperty.GetValue(responseEntity);
                Console.WriteLine($"[ExecuteBase] TResponse type: {typeof(TResponse)}");

#pragma warning disable CS8603 // Possible null reference return.
                return responseObjectPropertyValue;
#pragma warning restore CS8603 // Possible null reference return.
            }

            if (apiResponse.ErrorName == null || apiResponse.ErrorMessage == null) {
                throw new WrongResponseException(methodPath);
            }

            throw new MethodException(methodPath, apiResponse.ErrorName, apiResponse.ErrorMessage);
        }

        private async Task<string> _ExecuteBase(string methodPath, string? payload, bool withAccessToken, CancellationToken token)
        {
            HttpResponseMessage? response;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUri}{methodPath}");
            request.Content = new StringContent(payload ?? "", Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient()) {
                client.Timeout = TimeSpan.FromSeconds(40);
                client.DefaultRequestHeaders.Add("Accept", "application/json;charset=utf-8");
                client.DefaultRequestHeaders.Add("x-api-key", ApiCredentials.API_KEY);

                if (withAccessToken) {
                    if (AccessToken == null) {
                        throw new LccUserNotAuthorizedException();
                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                }

                try {
                    response = await client.SendAsync(request, token).ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode) {
                        if (response.StatusCode == HttpStatusCode.InternalServerError) {
                            throw new ApiServerException();
                        }
                    }
                } catch (HttpRequestException e) {
                    if (e.InnerException is SocketException sE) {
                        if (sE.SocketErrorCode == SocketError.ConnectionRefused) {
                            throw new ServerUnreachableException();
                        }

                        if (sE.SocketErrorCode is SocketError.HostUnreachable or SocketError.HostNotFound) {
                            throw new NetworkUnreachableException();
                        }
                    }

                    throw;
                } catch (TaskCanceledException e) {
                    if (e.InnerException is TimeoutException) {
                        throw new NetworkUnreachableException();
                    }

                    throw;
                }
            }

            string responseString = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
            Debug.WriteLine($"Executed {methodPath}. Request - {payload},  Response - {responseString} ");

            return responseString;
        }
    }
}