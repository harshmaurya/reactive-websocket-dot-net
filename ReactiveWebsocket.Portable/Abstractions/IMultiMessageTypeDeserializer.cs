namespace ReactiveWebsocket.Abstractions
{
    public interface IMultiMessageTypeDeserializer
    {
        bool TryDeserialize<TResultType>(byte[] bytes, out TResultType result);
    }
}