using LccApiNet.Core.Categories.Abstraction;
using LccApiNet.Model.LongPoll.Methods;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LccApiNet.Exceptions;

namespace LccApiNet.Core.Categories
{
    /// <inheritdoc />
    internal class LongPollCategory : ILongPollCategory
    {
        private ILccApi _api;

        public LongPollCategory(ILccApi api)
        {
            _api = api;
        }

        public async Task<LongPollEventsResponse> GetEvents(LongPollEventsParameters @params, CancellationToken token = default)
        {
            LongPollEventsResponse response = await _api.ExecuteAsync<LongPollEventsResponse, LongPollEventsParameters>("/longpoll/getEvents", @params, true, token);
            if (response.Events == null) {
                throw new WrongResponseException("/longpoll/getEvents");
            }

            return response;
        }
        
        public async Task<int> GetLastEventId(CancellationToken token = default)
        {
            EventIdResponse response = await _api.ExecuteAsync<EventIdResponse>("/longpoll/getLastEventId", true, token);
            return response.LastEventId;            
        }
    }
}
