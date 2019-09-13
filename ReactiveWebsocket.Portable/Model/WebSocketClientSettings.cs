using System;

namespace ReactiveWebsocket.Model
{
    public class WebSocketClientSettings
    {
        public WebSocketClientSettings(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }

        public bool AutoReconnect { get; set; }
    }
}