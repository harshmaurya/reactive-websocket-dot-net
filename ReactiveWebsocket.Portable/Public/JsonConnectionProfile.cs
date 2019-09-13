using ReactiveWebsocket.Abstractions;
using ReactiveWebsocket.Implementation;
using ReactiveWebsocket.Model;

namespace ReactiveWebsocket.Portable.Public
{
    public class JsonConnectionProfile : IConnectionProfile
    {
        public JsonConnectionProfile()
        {
            MessageType = MessageType.Text;
            Serializer = new JsonSerializer();
            Deserializer = new JsonDeserializer();
        }

        public MessageType MessageType { get; }
        public ISerializer Serializer { get; }
        public IDeserializer Deserializer { get; }
    }
}
