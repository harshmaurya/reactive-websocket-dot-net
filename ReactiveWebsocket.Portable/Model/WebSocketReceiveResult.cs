namespace ReactiveWebsocket.Model
{
    public class WebSocketReceiveResult
    {
        public WebSocketReceiveResult(int count, bool endOfMessage)
        {
            Count = count;
            EndOfMessage = endOfMessage;
        }

        public int Count { get; }

        public bool EndOfMessage { get; }
    }
}
