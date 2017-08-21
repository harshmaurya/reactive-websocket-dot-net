namespace ReactiveWebsocket.Abstractions
{
    public interface IMultiMessageTypeSerializer
    {
        byte[] Serialize<TRequestType>(TRequestType payload);
    }
    
}