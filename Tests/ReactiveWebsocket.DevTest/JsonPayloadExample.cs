using System;
using ReactiveWebsocket.Model;
using ReactiveWebsocket.Portable.Public;
using ReactiveWebsocket.Public;
using ReactiveWebsocket.Samples.Model;

namespace ReactiveWebsocket.Samples
{
    internal class JsonPayloadExample
    {
        private const string Uri = "ws://reactivewebsocket.azurewebsites.net/ReactiveWebSocketServer/jsondemo";

        public async void RunAsync()
        {
            var factory = new WebsocketClientFactory(PlatformName.Desktop);
            var socket = factory.CreateGeneric(new JsonConnectionProfile(), new WebSocketClientSettings(new Uri(Uri)));
            
            socket.StatusStream.Subscribe(value =>
            {
                Console.WriteLine(value.ConnectionState != ConnectionState.Disconnected
                    ? $"Status: {value.ConnectionState}; {value.Message}"
                    : $"Status: {value.ConnectionState}; {value.Error?.Message}");
            });
            
            var success = await socket.ConnectAsync();
            if (!success) return;

            var inputData = new Data()
            {
                Id = Guid.NewGuid().ToString(),
                Number1 = 100,
                Number2 = 200
            };
            Console.WriteLine("Sending request to server: ");

            var result = await socket.GetResponse<Data, Result>(inputData, result1 => result1.Id.Equals(inputData.Id));

            socket.GetObservable<Data, Result>(inputData, result1 => result1.Id.Equals(inputData.Id))
                .Subscribe(resultReceived =>
                    {
                        Console.WriteLine($"Result received from server {resultReceived.Value}");
                    },
                    exception =>
                    {
                        Console.WriteLine($"Error: {exception.Message}");
                    });
        }
    }
}
