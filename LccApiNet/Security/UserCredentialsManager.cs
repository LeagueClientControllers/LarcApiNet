using Newtonsoft.Json;

using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
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
        internal static async Task SaveAccessToken(string accessToken) {
            string credentialsFilePath = Path.Combine(Environment.CurrentDirectory, FILE_NAME);
            using (StreamWriter sw = new StreamWriter(File.Open(credentialsFilePath, FileMode.Create))) {
                await sw.WriteAsync(JsonConvert.SerializeObject(new UserCredentials(accessToken)));
            }
        }

        /// <summary>
        /// Gains saved access token from the system
        /// </summary>
        /// <returns>Stored access token</returns>
        internal static async Task<string?> GetAccessToken()
        {
            string credentialsFilePath = Path.Combine(Environment.CurrentDirectory, FILE_NAME);

            string? storedData;
            using (StreamReader sr = new StreamReader(File.Open(credentialsFilePath, FileMode.Open))) {
                storedData = await sr.ReadToEndAsync();
            }

            if (storedData == null) return null;

            try {
                return JsonConvert.DeserializeObject<UserCredentials>(storedData).Decode();
            } catch {
                return null;
            }
        }

        private class UserCredentials
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            /// <summary>
            /// Creates instance of the user credentials
            /// </summary>
            /// <param name="accessToken">Unencrypted access token</param>
            public UserCredentials(string accessToken)
            {
                byte[] encryptedData = ProtectedData.Protect(Encoding.ASCII.GetBytes(accessToken), null, DataProtectionScope.CurrentUser);
                AccessToken = Encoding.ASCII.GetString(encryptedData);
            }

            /// <summary>
            /// Decodes encrypted data into string
            /// </summary>
            /// <returns>Decrypted string</returns>
            public string Decode() =>
                Encoding.ASCII.GetString(
                    ProtectedData.Unprotect(Encoding.ASCII.GetBytes(AccessToken), null, DataProtectionScope.CurrentUser));
        }
    }
}
