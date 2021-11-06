using LccApi.Core;

using LccApiNet.Core.Categories.Abstraction;

using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
