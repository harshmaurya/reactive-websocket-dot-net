using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ReactiveWebsocket.Abstractions;
using ReactiveWebsocket.Helpers;
using ReactiveWebsocket.Model;

namespace ReactiveWebsocket.Implementation
{
    public class MultiMessageTypeWebsocket : IMultiMessageTypeWebsocket
    {
        private readonly IMultiMessageTypeSerializer _serializer;
        private readonly IMultiMessageTypeDeserializer _deserializer;
        private readonly IRawWebsocketCommunicator _communicator;

        public MultiMessageTypeWebsocket(IMultiMessageTypeSerializer serializer, IMultiMessageTypeDeserializer deserializer,
            IRawWebsocketCommunicator communicator)
        {
            _serializer = serializer;
            _deserializer = deserializer;
            _communicator = communicator;
        }


        private Status CurrentStatus => StatusStream.FirstAsync().Wait();

        public Task<TResponsePayload> GetResponse<TResponsePayload>(Predicate<TResponsePayload> filter)
        {
            return _communicator.GetResponseStream().VerifyConnected(CurrentStatus)
                .Select(bytes => TryDeSerialize<TResponsePayload>(bytes))
                .Where(payLoad => payLoad != null && filter(payLoad))
                .FirstAsync().ToTask();
        }

        public async Task<TResponsePayload> GetResponse<TRequestPayload, TResponsePayload>(TRequestPayload requestPayload, Predicate<TResponsePayload> filter)
        {
            if (CurrentStatus.ConnectionState != ConnectionState.Connected)
            {
                throw Extensions.NotconnectedError();
            }
            await SendRequestAsync(requestPayload);
            var reponse = await _communicator.GetResponseStream().Select(bytes => TryDeSerialize<TResponsePayload>(bytes))
                .Where(payLoad => payLoad != null && filter(payLoad))
                   .FirstAsync().ToTask();
            return reponse;
        }

        public IObservable<TResponseType> GetObservable<TRequestType, TResponseType>(TRequestType requestPayload, Predicate<TResponseType> filter)
        {
            return Observable.Create<TResponseType>(observer =>
            {
                if (CurrentStatus.ConnectionState != ConnectionState.Connected)
                {
                    observer.OnError(Extensions.NotconnectedError());
                    return Disposable.Empty;
                }
                SendRequestAsync(requestPayload)
                    .ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                            throw new Exception(task.Exception.Message); 
                    });
                return _communicator.GetResponseStream().VerifyConnected(CurrentStatus)
                .Select(bytes => TryDeSerialize<TResponseType>(bytes))
                .Where(payLoad => payLoad != null && filter(payLoad))
                 .Subscribe(observer);
            });
        }

        public IObservable<TResponseType> GetObservable<TRequestType, TResponseType>(Predicate<TResponseType> filter)
        {
            return _communicator.GetResponseStream().VerifyConnected(CurrentStatus)
                .Select(bytes => TryDeSerialize<TResponseType>(bytes))
                .Where(payLoad => payLoad != null && filter(payLoad));
        }

        public IObservable<Status> StatusStream => _communicator.StatusStream;

        public Task<bool> ConnectAsync()
        {
            return _communicator.ConnectAsync();
        }
        

        public Task CloseAsync()
        {
            return _communicator.CloseAsync();
        }

        public void Dispose()
        {
            _communicator.Dispose();
        }

        #region Private

        private TResponse TryDeSerialize<TResponse>(byte[] bytes)
        {
            return _deserializer.TryDeserialize(bytes, out TResponse result) ? result : default(TResponse);
        }

        private async Task SendRequestAsync<TRequestType>(TRequestType requestPayload)
        {
            var bytes = _serializer.Serialize(requestPayload);
            await _communicator.SendMessageAsync(bytes);
        }
        #endregion

    }
}
