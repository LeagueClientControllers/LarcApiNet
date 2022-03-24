using System;
using System.Threading;
using System.Threading.Tasks;

using LccApiNet.EventHandlers;
using LccApiNet.Model.LongPoll;
using LccApiNet.Model.LongPoll.Methods;

namespace LccApiNet.Services
{
    /// <summary>
    /// Simplifies work with long poll system
    /// and allows to get user events 
    /// </summary>
    public class UserEventService
    {
        private ILccApi _api;
        private int _lastEventId;

        /// <summary>
        /// Fired when exception has been occurred while long poll event loop.
        /// </summary>
        public event Action<Exception>? ExceptionOccurred;

        /// <summary>
        /// Invoked when new remote devices is authorized
        /// </summary>
        public event DeviceAddedEventHandler? DeviceAdded;

        /// <summary>
        /// Invoked when device was changed
        /// </summary>
        public event DeviceChangedEventHandler? DeviceChanged;

        /// <summary>
        /// Invoked when command was sent;
        /// </summary>
        public event CommandSentEventHandler? CommandSent;

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
            //_lastEventId = await _api.LongPoll.GetLastEventIdAsync(token);
            //_ = WorkingTask(workerToken);
        }

        private async Task WorkingTask(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested) {
                try {
                    LongPollEventsResponse response = await _api.LongPoll.GetEventsAsync(_lastEventId, token: token);
                    foreach (DeviceEvent de in response.Events.DeviceEvents) {
                        if (de.Type == DeviceEventType.DeviceAdded) {
                            DeviceAdded?.Invoke(this, new DeviceAddedEventArgs(de.DeviceId));
                        } else if (de.Type == DeviceEventType.DeviceChanged) {
                            DeviceChanged?.Invoke(this, new DeviceChangedEventArgs(de.DeviceId, de.Changes!));
                        }
                    }

                    foreach (CommandEvent ce in response.Events.CommandEvents) {
                        if (ce.Type == CommandEventType.CommandSent) {
                            CommandSent?.Invoke(this, new CommandSentEventArgs(ce.Command!));
                        }
                    }

                    _lastEventId = response.LastEventId;
                } catch (Exception e) {
                    ExceptionOccurred?.Invoke(e);
                    break;
                }
            }
        }
    }
}