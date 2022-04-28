using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Security
{

    public interface IUserCredentialsStorage
    {
        Task StoreAccessTokenAsync(string accessToken, CancellationToken token = default);
        Task<string?> RetrieveAccessTokenAsync(CancellationToken token = default);
        Task ClearAccessTokenAsync(CancellationToken token = default);
    }
}
