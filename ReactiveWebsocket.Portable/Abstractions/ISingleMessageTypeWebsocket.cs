using System;
using System.Threading.Tasks;

namespace ReactiveWebsocket.Abstractions
{
    /// <summary>
    /// Interface representing single type of message format for communication identified by
    /// <typeparam name="TRequestType"/> and <typeparam name="TResponseType"/>
    /// </summary>
    /// <typeparam name="TRequestType">Request type</typeparam>
    /// <typeparam name="TResponseType">Response type</typeparam>
    public interface ISingleMessageTypeWebsocket<in TRequestType, TResponseType>
    {
        Task<TResponseType> GetResponse(Predicate<TResponseType> filter);

        Task<TResponseType> GetResponse(TRequestType requestPayload, Predicate<TResponseType> filter);

        IObservable<TResponseType> GetObservable(TRequestType requestPayload, Predicate<TResponseType> filter);

        IObservable<TResponseType> GetObservable(Predicate<TResponseType> filter);

    }
}