namespace ReactiveWebsocket.Model
{
    public class WebSocketOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageType"> The type of message your payload supports(binary/text)</param>
        public WebSocketOptions(MessageType messageType)
        {
            MessageType = messageType;
        }

        /// <summary>
        /// Time interval to keep the connection alive after idle state. Typically it is 30 seconds,
        //  So the client sends a pong request 30 seconds after being idle.
        /// </summary>
        //public TimeSpan? KeepAliveInterval { get; set; }


        /// <summary>
        /// The type of message your payload supports (binary/text)
        /// </summary>
        public MessageType MessageType { get; }


        //public long BufferSize { get; set; }
    }
}