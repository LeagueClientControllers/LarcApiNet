using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using LccApiNet.EventHandlers;
using LccApiNet.Exceptions;
using LccApiNet.Security;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Websocket.Client;

namespace LccApiNet.Services
{
    /// <summary>
    /// Simplifies work with long poll system
    /// and allows to get user events 
    /// </summary>
    public partial class UserEventService : IDisposable
    {
        private ILccApi _api;
        private WebsocketClient? _socket;
        private Uri _webSocketUrl = new Uri($"ws://{ILccApi.API_HOST}/ws");

        public UserEventService(ILccApi api)
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

            JObject eventMessage;
            try {
                eventMessage = JObject.Parse(message.Text);
            } catch (JsonReaderException) {
                throw new EventProviderException($"Incoming message parsing error occurred.");
            }

            ;
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}