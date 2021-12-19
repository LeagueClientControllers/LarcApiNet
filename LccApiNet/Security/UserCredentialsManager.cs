using Newtonsoft.Json;

using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Security
{
    /// <summary>
    /// Stores the user credentials in the system and get it back
    /// </summary>
    internal static class UserCredentialsManager
    {
        private const string FILE_NAME = "credentials.json"; 

        /// <summary>
        /// Saves the access token in the system
        /// </summary>
        /// <param name="accessToken">access token that should be saved in the system</param>
        internal static async Task SaveAccessTokenAsync(string accessToken, CancellationToken token = default) {
            string credentialsFilePath = Path.Combine(Environment.CurrentDirectory, FILE_NAME);
            using (StreamWriter sw = new StreamWriter(File.Open(credentialsFilePath, FileMode.Create))) {
                await sw.WriteAsync(JsonConvert.SerializeObject(new UserCredentials(accessToken)).AsMemory(), token).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gains saved access token from the system
        /// </summary>
        /// <returns>Stored access token</returns>
        internal static async Task<string?> GetAccessTokenAsync()
        {
            string credentialsFilePath = Path.Combine(Environment.CurrentDirectory, FILE_NAME);

            if (!File.Exists(credentialsFilePath))
                return null;

            string? storedData;
            using (StreamReader sr = new StreamReader(File.Open(credentialsFilePath, FileMode.Open))) {
                storedData = await sr.ReadToEndAsync().ConfigureAwait(false);
            }

            if (storedData == null) return null;

            try {
                return JsonConvert.DeserializeObject<UserCredentials>(storedData)?.Decode();
            } catch (Exception e) {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Completely removes access token from the system
        /// </summary>
        internal static void ClearAccessToken()
        {
            string credentialsFilePath = Path.Combine(Environment.CurrentDirectory, FILE_NAME);

            if (File.Exists(credentialsFilePath))
                File.Delete(credentialsFilePath);
        }

        private class UserCredentials
        {
            [JsonProperty("accessToken")]
            public string? AccessToken { get; set; }

            /// <summary>
            /// Creates instance of the user credentials without encryption
            /// </summary>
            public UserCredentials() {}

            /// <summary>
            /// Creates instance of the user credentials
            /// </summary>
            /// <param name="accessToken">Unencrypted access token</param>
            public UserCredentials(string accessToken)
            {
                byte[] encryptedData = ProtectedData.Protect(Encoding.UTF8.GetBytes(accessToken), null, DataProtectionScope.CurrentUser);
                AccessToken = Convert.ToBase64String(encryptedData);
            }

            /// <summary>
            /// Decodes encrypted data into string
            /// </summary>
            /// <returns>Decrypted string</returns>
            public string Decode()
            {
                byte[] bytes = Convert.FromBase64String(AccessToken!);
                byte[] unprotected = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
                string decoded = Encoding.UTF8.GetString(unprotected);
                return decoded;
            }
        }
    }
}
