using System;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.Portable.Public;
using ReactiveWebsocket.Public;
using ConnectionState = ReactiveWebsocket.Model.ConnectionState;

namespace ReactiveWebsocket.Samples
{
    internal class StringPayloadExample
    {
        private const string Uri = "ws://reactivewebsocket.azurewebsites.net/ReactiveWebSocketServer/stringdemo";
        
        public async void RunAsync()
        {
            var factory = new WebsocketClientFactory(PlatformName.Desktop);
            var socket = factory.Create<string, string>(new StringConnectionProfile(), new WebSocketClientSettings(new Uri(Uri)));


            socket.StatusStream.Subscribe(value =>
            {
                Console.WriteLine(value.ConnectionState != ConnectionState.Disconnected
                    ? $"Status: {value.ConnectionState}; {value.Message}"
                    : $"Status: {value.ConnectionState}; {value.Error?.Message}");
            });

            var success = await socket.ConnectAsync();
            if (!success) return;

            const string request = "test";
            Console.WriteLine("Sending request to server: " + request);

            var result = await socket.GetResponse(request, s => true);


            socket.GetObservable(request, s => true)
                .Subscribe(value =>
                    {
                        Console.WriteLine($"Message received from server {value}");
                    },
                    exception =>
                    {
                        Console.WriteLine($"Error: {exception.Message}");
                    });
        }
    }
}
