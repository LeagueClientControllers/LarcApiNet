using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using LarcApiNet.Exceptions;
using LarcApiNet.Model.Local;
using LarcApiNet.Security;

using Newtonsoft.Json;
using Websocket.Client;

namespace LarcApiNet.Services
{
    /// <summary>
    /// Simplifies work with long poll system
    /// and allows to get user events 
    /// </summary>
    public partial class EventService : IDisposable
    {
        private WebsocketClient? _socket;
        private readonly ILarcApi _api;
        private readonly Uri _webSocketUrl = new Uri($"ws://{ILarcApi.API_HOST}/ws");

        public EventService(ILarcApi api)
        {
            _api = api;
        }

        public async Task ConnectToEventProviderAsync(CancellationToken token = default)
        {
            _socket = new WebsocketClient(_webSocketUrl, () => { 
                ClientWebSocket socket = new ClientWebSocket {
                    Options = {
                        RemoteCertificateValidationCallback = (_, _, _, _) => true,
                    },
                };

                socket.Options.SetRequestHeader("x-api-key", ApiCredentials.API_KEY);
                socket.Options.SetRequestHeader("Authorization", $"Bearer {_api.AccessToken}");
                return socket;
            });

            _socket.MessageReceived.Subscribe(HandleWebSocketMessage);
            await _socket.Start();
        }

        private void HandleWebSocketMessage(ResponseMessage message)
        {
            if (message.MessageType != WebSocketMessageType.Text) {
                throw new EventProviderException($"Incoming message type is '{message.MessageType}' that is invalid.");
            }

            EventMessage? eventMessage;
            try {
                eventMessage = JsonConvert.DeserializeObject<EventMessage>(message.Text);
            } catch (JsonReaderException) {
                throw new EventProviderException($"Incoming message parsing error occurred.");
            }

            if (eventMessage is null) {
                throw new EventProviderException($"Incoming message is missing.");
            }

            HandleEventMessage(eventMessage);
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}