using System;
using System.Threading.Tasks;

namespace ReactiveWebsocket.Abstractions
{
    /// <summary>
    /// Interface representing websocket client
    /// </summary>
    /// <typeparam name="TRequestType">type used for sending request</typeparam>
    /// <typeparam name="TResponseType">type used for getting response</typeparam>
    public interface IWebSocketClient<in TRequestType, TResponseType> : IConnectableWebsocket
    {
        Task<TResponseType> GetResponse(Predicate<TResponseType> filter);

        Task<TResponseType> GetResponse(TRequestType requestPayload, Predicate<TResponseType> filter);

        IObservable<TResponseType> GetObservable(TRequestType requestPayload, Predicate<TResponseType> filter);

        IObservable<TResponseType> GetObservable(Predicate<TResponseType> filter);

    }

    public interface IWebSocketClient : IConnectableWebsocket
    {

        /// <summary>
        /// Gets a single response filtered by given predicate condition
        /// </summary>
        /// <param name="filter">The filter predicate to identify the response</param>
        /// <returns></returns>
        Task<TResponseType> GetResponse<TResponseType>(Predicate<TResponseType> filter);


        /// <summary>
        /// Sends a request and gets a single response filtered by given predicate condition
        /// </summary>
        /// <param name="requestPayload"></param>
        /// <param name="filter">The filter predicate to identify the response</param>
        /// <returns></returns>
        Task<TResponseType> GetResponse<TRequestType, TResponseType>(TRequestType requestPayload, Predicate<TResponseType> filter);


        /// <summary>
        /// Sends a request and gets a stream of response filtered by given predicate condition
        /// </summary>
        /// <param name="requestPayload"></param>
        /// <param name="filter">The filter predicate to identify the response</param>
        /// <returns></returns>
        IObservable<TResponseType> GetObservable<TRequestType, TResponseType>(TRequestType requestPayload, Predicate<TResponseType> filter);


        /// <summary>
        /// Sends a request and gets a stream of response filtered by given predicate condition
        /// </summary>
        /// <param name="filter">The filter predicate to identify the response</param>
        /// <returns></returns>
        IObservable<TResponseType> GetObservable<TRequestType, TResponseType>(Predicate<TResponseType> filter);

    }
}