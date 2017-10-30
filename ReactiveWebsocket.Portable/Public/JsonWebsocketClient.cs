using System;
using ReactiveWebsocket.Implementation;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.PlatformAbstraction;

namespace ReactiveWebsocket.Public
{
    public class JsonWebsocketClient : MultiMessageTypeWebsocket
    {
        public JsonWebsocketClient(Uri uri) :
            base(new JsonSerializer(), new JsonDeserializer(),
                new RawWebsocketClient(PlatformHelper.Resolve<IPlatformWebsocket>(),
                    uri, new WebSocketOptions(MessageType.Text)))
        {
        }
    }
}