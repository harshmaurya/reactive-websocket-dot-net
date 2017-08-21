namespace ReactiveWebsocket.Abstractions
{
    public interface ISingleMessageTypeDeserializer<TResultType>
    {
        bool TryDeserialize(byte[] bytes, out TResultType result);
    }
}