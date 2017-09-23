using ReactiveWebsocket.Abstractions;

namespace ReactiveWebsocket.Implementation
{
    public class JsonSerializer : IMultiMessageTypeSerializer
    {
        public byte[] Serialize<TRequestType>(TRequestType payload)
        {
            var serializer = new JsonSerializer();
            return serializer.Serialize(payload);
        }
    }
}