using LccApi.Core;

using LccApiNet.Core.Categories;
using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Exceptions;
using LccApiNet.Security;

using Newtonsoft.Json;

using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LccApiNet.Core
{
    /// <inheritdoc />
    class LccApi : ILccApi
    {
        ///Клоунада

        /// <inheritdoc />
        public string? AccessToken { get; private set; }

        /// <inheritdoc />
        public IIdentityCategory Identity { get; private set; }

        public LccApi()
        {
            Identity = new IdentityCategory(this);
        }

        /// <inheritdoc />
        public async Task<TResponse> ExecuteAsync<TResponse, TParameters>(string methodPath, TParameters @params, bool withAccessToken = true)
        {
            HttpWebRequest request = WebRequest.CreateHttp(Path.Combine("http://151.248.115.14", methodPath));
            request.Method = "GET";
            request.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=utf-8;");
            request.Headers.Add(HttpRequestHeader.Connection, "keep-alive");
            request.Headers.Add("x-api-key", ApiCredentials.API_KEY);

            if (withAccessToken) {
                if (AccessToken == null) throw new LccUserNotAuthorizedException();

                request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {AccessToken}");
            }

            using (StreamWriter sw = new StreamWriter(request.GetRequestStream())) {
                await sw.WriteAsync(JsonConvert.SerializeObject(@params));
            }

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            string responseBody;
            
            using (StreamReader sr = new StreamReader(response.GetResponseStream())) {
                responseBody = await sr.ReadToEndAsync();
            }

            return default(TResponse);

        }
    }
}
