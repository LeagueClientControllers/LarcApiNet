﻿using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;

using LccApiNet.Core.Categories;
using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Enums.Safety;
using LccApiNet.Exceptions;
using LccApiNet.Model.General;
using LccApiNet.Security;

using Newtonsoft.Json;

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core
{
    /// <summary>
    /// Main API class
    /// </summary>
    public class LccApi : ILccApi
    {
        private const string API_HOST = "150.230.151.8";
        private const int PORT = 56032;

        private JwtPayload? _accessTokenContent;
        private Uri _baseUri = new Uri($"http://{API_HOST}:{PORT}");

        /// <inheritdoc />
        public string? AccessToken { get; private set; }

        /// <inheritdoc />
        public IIdentityCategory Identity { get; private set; }

        /// <inheritdoc />
        public IDeviceCategory Device { get; private set; }

        /// <summary>
        /// Creates new instance of the main API class
        /// </summary>
        public LccApi()
        {
            Identity = new IdentityCategory(this);
            Device = new DeviceCategory(this);
        }

        /// <inheritdoc />
        public async Task InitAsync(CancellationToken token = default)
        {
            string? accessToken = await UserCredentialsManager.GetAccessTokenAsync().ConfigureAwait(false);
            if (accessToken == null)
                return;

            AccessToken = accessToken;

            try {
                await Identity.RefreshAccessTokenAsync(token).ConfigureAwait(false);
            } catch (MethodException me) {
                if (me.ErrorName == MethodError.WrongAccessToken) {
                    AccessToken = null;
                    UserCredentialsManager.ClearAccessToken();
                }
            }
        }

        /// <inheritdoc />
        public async Task<TResponse> ExecuteAsync<TResponse, TParameters>(string methodPath, TParameters @params, bool withAccessToken = true, CancellationToken token = default)
            where TResponse : ApiResponse
        {
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(_baseUri, methodPath));
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            request.Accept = "application/json;charset=utf-8";
            request.Headers.Add("x-api-key", ApiCredentials.API_KEY);

            if (withAccessToken) {
                if (AccessToken == null) throw new LccUserNotAuthorizedException();

                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {AccessToken}");
            }
            
            string payload = JsonConvert.SerializeObject(@params);
            using (Stream requestStream = request.GetRequestStream())
            using (StreamWriter sw = new StreamWriter(requestStream)) {
                await sw.WriteAsync(payload.AsMemory(), token).ConfigureAwait(false);
                await sw.FlushAsync().ConfigureAwait(false);
            }

            HttpWebResponse? response;
            using (token.Register(() => request.Abort(), useSynchronizationContext: false)) {
                try {
                    response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                } catch (WebException e) {
                    response = (HttpWebResponse)e.Response;
                    if (response == null && token.IsCancellationRequested) {
                        throw new OperationCanceledException(e.Message, e, token);
                    }
                }
            }

            string responseBody;
            using (StreamReader sr = new StreamReader(response!.GetResponseStream())) {
                responseBody = await sr.ReadToEndAsync().ConfigureAwait(false);
            }

            TResponse? responseEntity = JsonConvert.DeserializeObject<TResponse>(responseBody);
            Debug.WriteLine($"Executed {methodPath}. Request - {payload},  Response - {{{JsonConvert.SerializeObject(responseEntity, Formatting.Indented)}}} ");
            
            if (responseEntity == null) throw new MissingResponseException(methodPath);
            if (responseEntity.Result == ExecutionResult.Error) {
                if (responseEntity.ErrorName == null || responseEntity.ErrorMessage == null)
                    throw new WrongResponseException(methodPath);
                else
                    throw new MethodException(methodPath, responseEntity.ErrorName, responseEntity.ErrorMessage);
            }

            return responseEntity;
        }

        /// <inheritdoc />
        public async Task<TResponse> ExecuteAsync<TResponse>(string methodPath, bool withAccessToken = true, CancellationToken token = default)
            where TResponse : ApiResponse
        {
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(_baseUri, methodPath));
            request.Method = "POST";
            request.Accept = "application/json;charset=utf-8";
            request.Headers.Add("x-api-key", ApiCredentials.API_KEY);

            if (withAccessToken) {
                if (AccessToken == null) throw new LccUserNotAuthorizedException();

                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {AccessToken}");
            }

            HttpWebResponse? response;
            using (token.Register(() => request.Abort(), useSynchronizationContext: false)) {
                try {
                    response = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false);
                } catch (WebException e) {
                    response = (HttpWebResponse)e.Response;
                    if (response.StatusCode == HttpStatusCode.InternalServerError) {
                        throw new ApiServerException();
                    }
                    
                    if (response == null && token.IsCancellationRequested) {
                        throw new OperationCanceledException(e.Message, e, token);
                    }
                }
            }

            string responseBody;
            using (StreamReader sr = new StreamReader(response!.GetResponseStream())) {
                responseBody = await sr.ReadToEndAsync().ConfigureAwait(false);
            }

            TResponse? responseEntity = JsonConvert.DeserializeObject<TResponse>(responseBody);
            Debug.WriteLine($"Executed {methodPath}. Response - {{{JsonConvert.SerializeObject(responseEntity, Formatting.Indented)}}} ");

            if (responseEntity == null) throw new MissingResponseException(methodPath);
            if (responseEntity.Result == ExecutionResult.Error) {
                if (responseEntity.ErrorName == null || responseEntity.ErrorMessage == null)
                    throw new WrongResponseException(methodPath);
                else
                    throw new MethodException(methodPath, responseEntity.ErrorName, responseEntity.ErrorMessage);
            }

            return responseEntity;
        }

        /// <inheritdoc />
        public async Task UpdateAccessToken(string accessToken, bool storeInSystem = false)
        {
            AccessToken = accessToken;
            if (storeInSystem)
                await UserCredentialsManager.SaveAccessTokenAsync(accessToken).ConfigureAwait(false);

            _accessTokenContent = 
                JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(ApiCredentials.JWT_SECRET)
                    .DoNotVerifySignature()
                    .Decode<JwtPayload>(AccessToken);
        }
    }
}