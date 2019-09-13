using ReactiveWebsocket.Abstractions;
using ReactiveWebsocket.Implementation;
using ReactiveWebsocket.Model;

namespace ReactiveWebsocket.Portable.Public
{
    public class StringConnectionProfile : IConnectionProfile
    {
        public StringConnectionProfile()
        {
            MessageType = MessageType.Text;
            Serializer = new StringSerializer();
            Deserializer = new StringDeserializer();
        }

        public MessageType MessageType { get; }
        public ISerializer Serializer { get; }
        public IDeserializer Deserializer { get; }
    }
}