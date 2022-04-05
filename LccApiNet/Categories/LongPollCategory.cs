//using LccApiNet.Categories.Abstraction;
//using LccApiNet.Model.LongPoll.Methods;

//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using LccApiNet.Exceptions;

//namespace LccApiNet.Categories
//{
//    /// <inheritdoc />
//    internal class LongPollCategory : ILongPollCategory
//    {
//        private ILccApi _api;

//        public LongPollCategory(ILccApi api)
//        {
//            _api = api;
//        }

//        /// <inheritdoc />
//        public async Task<int> GetLastEventIdAsync(CancellationToken token = default)
//        {
//            //EventIdResponse response = await _api.ExecuteAsync<EventIdResponse>("/longpoll/getLastEventId", true, token);
//            int? response = await _api.ExecuteAsync<int?>("/longpoll/getLastEventId", "eventId", true, token).ConfigureAwait(false);
//            if (response != null) {
//                return (int)response;            
//            } else {
//                throw new WrongResponseException("/longpoll/getLastEventId");
//            }
//        }
        
//        /// <inheritdoc />
//        public async Task<LongPollEventsResponse> GetEventsAsync(int lastEventId, int timeout = 30, CancellationToken token = default)
//        {
//            LongPollEventsParameters @params = new LongPollEventsParameters(lastEventId, timeout);
//            LongPollEventsResponse response = await _api.ExecuteAsync<LongPollEventsResponse, LongPollEventsParameters>("/longpoll/getEvents", @params, true, token).ConfigureAwait(false);
//            if (response.Events == null)
//            {
//                throw new WrongResponseException("/longpoll/getEvents");
//            }

//            return response;
//        }
//    }
//}
