using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LccApiNet.Security
{

    public interface IUserCreditionalsStorage
    {
        Task StoreAccessTokenAsync(string AccessToken);
        Task<string?> RetrieveAccessTokenAsync();
        Task ClearAccessTokenAsync();
    }
}
