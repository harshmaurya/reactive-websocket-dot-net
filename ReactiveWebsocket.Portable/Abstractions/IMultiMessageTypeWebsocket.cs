using System;
using System.Threading.Tasks;

namespace ReactiveWebsocket.Abstractions
{
    /// <summary>
    /// Interface representing communication with different message format each time
    /// </summary>
    public interface IMultiMessageTypeWebsocket : IConnectableWebsocket
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