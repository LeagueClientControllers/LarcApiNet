using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Model.LongPoll.Methods;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Core.Categories
{
    /// <inheritdoc />
    internal class LongPollCategory : ILongPollCategory
    {
        public Task<LongPollEventsResponse> GetEvents(LongPollEventsParameters @params, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
        
        public Task<int> GetLastEventId(CancellationToken token = default) 
        { 
            throw new NotImplementedException();
        }
    }
}
