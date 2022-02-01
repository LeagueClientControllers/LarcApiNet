using LccApiNet.Model.LongPoll.Methods;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.Categories.Abstraction
{
    /// <summary>
    /// Contains methods of the /longpoll/ API category 
    /// </summary>
    public interface ILongPollCategory
    {
        /// <summary>
        /// Gets id of the last current event of the user
        /// </summary>
        Task<int> GetLastEventId(CancellationToken token = default);

        /// <summary>
        /// Gets collection of the events happened after event specified as <see cref="LongPollEventsParameters.LastEventId"/>
        /// or listening to the incoming events during <see cref="LongPollEventsParameters.Timeout"/> and returns collection of the one event
        /// </summary>
        /// <param name="params">Parameters of the method</param>
        Task<LongPollEventsResponse> GetEvents(int lastEventId, int timeout = 60, CancellationToken token = default);
    }
}
