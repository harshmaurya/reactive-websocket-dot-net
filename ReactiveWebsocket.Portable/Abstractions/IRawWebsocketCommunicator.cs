using System;
using System.Threading.Tasks;

namespace ReactiveWebsocket.Abstractions
{
    public interface IRawWebsocketCommunicator : IConnectableWebsocket
    {
        /// <summary>
        /// Get the reponse stream of raw byte array
        /// </summary>
        /// <returns></returns>
        IObservable<byte[]> GetResponseStream();

        /// <summary>
        /// Send the message as a raw byte array
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageAsync(byte[] message);
    }
}