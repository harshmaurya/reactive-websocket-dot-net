using System;
using System.Threading;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.Public;
using ConnectionState = ReactiveWebsocket.Model.ConnectionState;

namespace ReactiveWebsocket.Samples
{
    internal class StringPayloadExample
    {
        private const string Uri = "ws://reactivewebsocket.azurewebsites.net/ReactiveWebSocketServer/stringdemo";
        
        public async void RunAsync()
        {
            //set platform assembly (Optional)
            WebsocketInitializer.SetPlatform(PlatformName.Desktop);

            // Use the StringWebsocketClient for simple string payloads
            var socket = new StringWebsocketClient();

            socket.StatusStream.Subscribe(value =>
            {
                Console.WriteLine(value.ConnectionState != ConnectionState.Disconnected
                    ? $"Status: {value.ConnectionState}; {value.Message}"
                    : $"Status: {value.ConnectionState}; {value.Error?.Message}");
            });

            var success = await socket.ConnectAsync(new Uri(Uri), CancellationToken.None);
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
