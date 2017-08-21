using System;
using System.Threading;
using System.Threading.Tasks;
using ReactiveWebsocket.Model;

namespace ReactiveWebsocket.PlatformAbstraction
{
    public interface IPlatformWebsocket : IDisposable
    {
        Task ConnectAsync(Uri uri, CancellationToken token);

        Task SendAsync(ArraySegment<byte> buffer, MessageType messageType, bool endOfMessage,
            CancellationToken cancellationToken);

        Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken token);

        Task CloseAsync();

        WebSocketState State { get; }
    }
}
