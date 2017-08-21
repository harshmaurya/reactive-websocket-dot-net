using System;
using System.Threading;
using System.Threading.Tasks;
using ReactiveWebsocket.Model;

namespace ReactiveWebsocket.Abstractions
{
    public interface IConnectableWebsocket : IDisposable
    {
        /// <summary>
        /// Status stream for the websocket connection
        /// </summary>
        IObservable<Status> StatusStream { get; }


        /// <summary>
        /// Connects to the websocket server
        /// </summary>
        /// <param name="uri">uri to connect to</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns></returns>
        Task<bool> ConnectAsync(Uri uri, CancellationToken cancellationToken);

        /// <summary>
        /// Reconnect to the given websocket server. Previous connection is disposed
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> Reconnect(Uri uri, CancellationToken cancellationToken);


        /// <summary>
        /// Closes the websocket connection
        /// </summary>
        /// <returns></returns>
        Task CloseAsync();
    }
}