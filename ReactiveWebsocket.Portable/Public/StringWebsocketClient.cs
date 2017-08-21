using ReactiveWebsocket.Implementation;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.PlatformAbstraction;

namespace ReactiveWebsocket.Public
{
    public class StringWebsocketClient : SingleMessageTypeWebsocket<string, string>
    {
        public StringWebsocketClient() :
            base(new StringSerializer(), new StringDeserializer(),
                new RawWebsocketClient(PlatformHelper.Resolve<IPlatformWebsocket>(), new WebSocketOptions(MessageType.Text)))
        {

        }
    }
}
