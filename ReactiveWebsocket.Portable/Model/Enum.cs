namespace ReactiveWebsocket.Model
{
    public enum PlatformName
    {
        Desktop,
        Android,
        Ios
    }

    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
    }
    public enum MessageType
    {
        Text,
        Binary
    }

    public enum WebSocketState
    {
        None,
        Connecting,
        Open,
        CloseSent,
        CloseReceived,
        Closed,
        Aborted,
    }
}
