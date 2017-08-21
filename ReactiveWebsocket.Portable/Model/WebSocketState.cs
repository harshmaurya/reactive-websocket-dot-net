namespace ReactiveWebsocket.Model
{
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