using ReactiveWebsocket.Model;

namespace ReactiveWebsocket.Abstractions
{
    public interface IConnectionProfile
    {
        MessageType MessageType { get; }

        ISerializer Serializer { get; }

        IDeserializer Deserializer { get; }
    }
}