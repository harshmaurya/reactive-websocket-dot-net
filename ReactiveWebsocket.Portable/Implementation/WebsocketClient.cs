using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ReactiveWebsocket.Abstractions;
using ReactiveWebsocket.Helpers;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.PlatformAbstraction;

namespace ReactiveWebsocket.Implementation
{
    internal class WebsocketClient : IWebSocketClient
    {
        private readonly ISerializer _serializer;
        private readonly IDeserializer _deserializer;
        private readonly WebsocketCommunicator _communicator;
        private readonly WebSocketClientSettings _settings;

        public WebsocketClient(IConnectionProfile profile,
            WebSocketClientSettings settings)
        {
            _serializer = profile.Serializer;
            _deserializer = profile.Deserializer;
            _communicator = new WebsocketCommunicator(
                PlatformHelper.Resolve<IPlatformWebsocket>(),
                profile.MessageType, settings.Uri);
            _settings = settings;
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

    public class WebsocketClient<TRequestType, TResponseType> : IWebSocketClient<TRequestType, TResponseType>
    {
        private readonly WebsocketClient _client;

        public WebsocketClient(IConnectionProfile profile,
            WebSocketClientSettings settings)
        {
            _client = new WebsocketClient(profile, settings);
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public IObservable<Status> StatusStream => _client.StatusStream;

        public Task<bool> ConnectAsync()
        {
            return _client.ConnectAsync();
        }

        public Task CloseAsync()
        {
            return _client.CloseAsync();
        }

        public Task<TResponseType> GetResponse(Predicate<TResponseType> filter)
        {
            return _client.GetResponse(filter);
        }

        public Task<TResponseType> GetResponse(TRequestType requestPayload, Predicate<TResponseType> filter)
        {
            return _client.GetResponse(requestPayload, filter);
        }

        public IObservable<TResponseType> GetObservable(TRequestType requestPayload, Predicate<TResponseType> filter)
        {
            return _client.GetObservable(requestPayload, filter);
        }

        public IObservable<TResponseType> GetObservable(Predicate<TResponseType> filter)
        {
            return _client.GetObservable<TRequestType, TResponseType>(filter);
        }
    }
}