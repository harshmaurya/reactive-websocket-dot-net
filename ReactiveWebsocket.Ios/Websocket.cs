using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.PlatformAbstraction;
using WebSocketReceiveResult = ReactiveWebsocket.Model.WebSocketReceiveResult;
using WebSocketState = ReactiveWebsocket.Model.WebSocketState;

namespace ReactiveWebsocket.Ios
{
    internal class Websocket : IPlatformWebsocket
    {
        private readonly ClientWebSocket _underlyingWebsocket = new ClientWebSocket();

        public void Dispose()
        {
            _underlyingWebsocket.Dispose();
        }

        public Task ConnectAsync(Uri uri, CancellationToken token)
        {
            return _underlyingWebsocket.ConnectAsync(uri, token);
        }

        public Task SendAsync(ArraySegment<byte> buffer, MessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            var mType = messageType == MessageType.Binary
                ? WebSocketMessageType.Binary
                    : WebSocketMessageType.Text;
            return _underlyingWebsocket.SendAsync(buffer, mType, endOfMessage, CancellationToken.None);
        }

        public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken token)
        {
            return _underlyingWebsocket.ReceiveAsync(buffer, token).ContinueWith(task =>
            {
                var result = task.Result;
                return new WebSocketReceiveResult(result.Count, result.EndOfMessage);
            }, token);
        }

        public Task CloseAsync()
        {
            return _underlyingWebsocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty,
                CancellationToken.None);
        }

        public WebSocketState State => (WebSocketState)_underlyingWebsocket.State;
    }

}
