using ReactiveWebsocket.Implementation;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.PlatformAbstraction;

namespace ReactiveWebsocket.Public
{
    public class JsonWebsocketClient : MultiMessageTypeWebsocket
    {
        public JsonWebsocketClient() :
            base(new JsonSerializer(), new JsonDeserializer(),
                new RawWebsocketClient(PlatformHelper.Resolve<IPlatformWebsocket>(),
                    new WebSocketOptions(MessageType.Text)))
        {
        }
    }
}