namespace ReactiveWebsocket.Abstractions
{
    public interface IDeserializer
    {
        bool TryDeserialize<TResultType>(byte[] bytes, out TResultType result);
    }
}