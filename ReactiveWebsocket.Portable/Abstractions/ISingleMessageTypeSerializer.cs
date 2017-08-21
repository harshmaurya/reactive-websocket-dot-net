namespace ReactiveWebsocket.Abstractions
{
    public interface ISingleMessageTypeSerializer<in TRequestType>
    {
        byte[] Serialize(TRequestType payload);
    }
}