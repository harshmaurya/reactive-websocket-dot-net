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
    public class SingleMessageTypeWebsocket<TRequestType, TResponseType> : ISingleMessageTypeWebsocket<TRequestType, TResponseType>
    {
        private readonly ISingleMessageTypeSerializer<TRequestType> _serializer;
        private readonly ISingleMessageTypeDeserializer<TResponseType> _deserializer;
        private readonly IRawWebsocketCommunicator _communicator;
        private Status CurrentStatus => StatusStream.FirstAsync().Wait();

        public SingleMessageTypeWebsocket(ISingleMessageTypeSerializer<TRequestType> serializer, ISingleMessageTypeDeserializer<TResponseType> deserializer,
            IRawWebsocketCommunicator communicator)
        {
            _serializer = serializer;
            _deserializer = deserializer;
            _communicator = communicator;
        }

        public void Dispose()
        {
            _communicator.Dispose();
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

        public Task<TResponseType> GetResponse(Predicate<TResponseType> filter)
        {
            return _communicator.GetResponseStream().VerifyConnected(CurrentStatus)
                .Select(TryDeSerialize)
                .Where(payLoad => payLoad != null && filter(payLoad))
                .FirstAsync().ToTask();
        }

        public async Task<TResponseType> GetResponse(TRequestType requestPayload, Predicate<TResponseType> filter)
        {
            if (CurrentStatus.ConnectionState != ConnectionState.Connected)
            {
                throw Extensions.NotconnectedError();
            }
            await SendRequestAsync(requestPayload);
            var reponse = await _communicator.GetResponseStream().Select(bytes => TryDeSerialize(bytes))
                .Where(payLoad => payLoad != null && filter(payLoad))
                .FirstAsync().ToTask();
            return reponse;
        }

        public IObservable<TResponseType> GetObservable(TRequestType requestPayload, Predicate<TResponseType> filter)
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
                    .Select(bytes => TryDeSerialize(bytes))
                    .Where(payLoad => payLoad != null && filter(payLoad))
                    .Subscribe(observer);
            });
        }

        public IObservable<TResponseType> GetObservable(Predicate<TResponseType> filter)
        {
            return _communicator.GetResponseStream().VerifyConnected(CurrentStatus)
                .Select(bytes => TryDeSerialize(bytes))
                .Where(payLoad => payLoad != null && filter(payLoad));
        }

        #region Private

        private TResponseType TryDeSerialize(byte[] bytes)
        {
            return _deserializer.TryDeserialize(bytes, out TResponseType result) ? result : default(TResponseType);
        }

        private async Task SendRequestAsync(TRequestType requestPayload)
        {
            var bytes = _serializer.Serialize(requestPayload);
            await _communicator.SendMessageAsync(bytes);
        }
        #endregion
    }
}