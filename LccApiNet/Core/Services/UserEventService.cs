using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LccApiNet.Model.LongPoll;
using LccApiNet.Model.LongPoll.Methods;

namespace LccApiNet.Core.Services
{
    /// <summary>
    /// Simplifies work with long poll system
    /// and allows to get user events 
    /// </summary>
    public class UserEventService
    {
        private ILccApi _api;
        private int lastEventId;

        /// <summary>
        /// Invoked when new remote devices is authorized
        /// </summary>
        public event Func<int, Task>? DeviceAdded;
        
        /// <summary>
        /// Invoked when device was changed
        /// </summary>
        public event Func<int, Dictionary<string, object>, Task>? DeviceChanged;
        
        public UserEventService(ILccApi api)
        {
            _api = api;
        }
        
        /// <summary>
        /// Starts the process of getting new events
        /// and handling them
        /// <param name="token">Token to be able to cancel initialization task</param>
        /// <param name="workerToken">Token to be able to cancel events handling</param>
        /// </summary>
        public async Task StartAsync(CancellationToken token = default, CancellationToken workerToken = default)
        {
            lastEventId = await _api.LongPoll.GetLastEventId(token);
            _ = WorkingTask(workerToken);
        }

        private async Task WorkingTask(CancellationToken token = default)
        {
            LongPollEventsParameters @params = new LongPollEventsParameters(lastEventId);
            
            while (!token.IsCancellationRequested) {
                LongPollEventsResponse response = await _api.LongPoll.GetEvents(@params, token);
                foreach (DeviceEvent de in response.Events.DeviceEvents) {
                    if (de.Type == DeviceEventType.DeviceAdded) {
                        _ = DeviceAdded?.Invoke(de.DeviceId);
                    } else if (de.Type == DeviceEventType.DeviceChanged) {
                        _ = DeviceChanged?.Invoke(de.DeviceId, de.Changes!);
                    }
                }
                
                @params.LastEventId = response.LastEventId;
            }
        } 
    }
}