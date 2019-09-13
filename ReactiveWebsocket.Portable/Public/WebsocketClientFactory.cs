using ReactiveWebsocket.Abstractions;
using ReactiveWebsocket.Implementation;
using ReactiveWebsocket.Model;

namespace ReactiveWebsocket.Public
{
    public class WebsocketClientFactory
    {
        public WebsocketClientFactory(PlatformName platformName)
        {
            WebsocketInitializer.SetPlatform(platformName);
        }

        public IWebSocketClient<TRequestType, TResponseType> Create<TRequestType, TResponseType>(
            IConnectionProfile profile, WebSocketClientSettings settings)
        {
            return new WebsocketClient<TRequestType, TResponseType>(profile, settings);
        }

        public IWebSocketClient CreateGeneric(
            IConnectionProfile profile, WebSocketClientSettings settings)
        {
            return new WebsocketClient(profile, settings);
        }

    }
}