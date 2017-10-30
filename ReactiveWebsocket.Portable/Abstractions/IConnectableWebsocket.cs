using System;
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
        /// 
        /// <returns></returns>
        Task<bool> ConnectAsync();

        
        /// <summary>
        /// Closes the websocket connection
        /// </summary>
        /// <returns></returns>
        Task CloseAsync();
    }
}