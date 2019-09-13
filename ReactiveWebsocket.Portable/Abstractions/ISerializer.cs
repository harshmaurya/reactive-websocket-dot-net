namespace ReactiveWebsocket.Abstractions
{
    public interface ISerializer
    {
        byte[] Serialize<TRequestType>(TRequestType payload);
    }
}