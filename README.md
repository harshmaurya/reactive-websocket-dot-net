Welcome to project home for reactive websocket - A portable .Net library providing client side websocket API. 

# Reactive websocket - Overview
Web-sockets are the latest addition to the communication protocol which standardizes the way client communicates with the server. It provides full duplex communication channel over a single TCP connection which is great considering the workarounds that existed previously for achieving the same result.For .Net world, Microsoft has provided a client implementation of WebSocket which can connect to any server supporting the same protocol. However, it is at a much lower level leaving the developer to deal with a lot of boilerplate code, not to mention effort which goes into making the code robust.Reactive WebSocket is an open source library which has been developed as a wrapper of the Microsoft’s implementation providing the following functionalities:
Based on a portable library, supports desktop and Android client. Other platforms coming soon.
Built-in Rx support. Supports both request-response and streaming style of communication.
Supports simple string, JSON as well as custom message serialization/deserialization.
Completely asynchronous API with no performance bottleneck at all.
Built-in exception handling and status reporting.
Supports multiple message type/format on the same stream

***

# Features
* Completely Asynchronous without any bottlenecks
* High performant and scalable
* Native Rx support
* Variety of platforms supported - Desktop, Android, Ios, and Xamarin Forms
* Built in support for string and JSON message format
* Support for custom serialization/deserialization


# Download

The library is available on NuGet. Based on which client you are targeting, you need to choose the corresponding library.

**For Desktop client:**

` Install-Package ReactiveWebsocket.Desktop -Version 1.0.0`

**For Android client:**

`Install-Package ReactiveWebsocket.Android -Version 1.0.0`

**For Ios client:**

`Install-Package ReactiveWebsocket.Ios -Version 1.0.0`

Using the above libraries, it is also possible to use them in all types of Xamarin projects. The [sample](https://github.com/harshmaurya/reactive-websocket-samples) programs provided showcase the features of the library.

# Usage:

The API is very easy to use. You just need to decide two things before starting
1. What type of serialization/deserialization you want to use? Simple string and JSON is available out of the box. You can use your own custom implementation if you want.
2. What is your messaging strategy? Are you exchanging the same **type** of message every time (using some kind of message wrapper class) or different types of messages?


First you need to create a factory by specifying the current platform which you are running on. 

`var factory = new WebsocketClientFactory(PlatformName.Desktop);`

Then get hold of the websocket object by specifying the parameters. There are two overloads in the factory method depending on what kind of messaging strategy you have decided to use as discussed in point 2 above.
In the below example we have used JSON profile and generic overload which means you are free to use any **type** of message for communication.
`var socket = factory.CreateGeneric(new JsonConnectionProfile(), new WebSocketClientSettings(new Uri(Uri)));`

Then you simply need to connect to the server.

`var success = await socket.ConnectAsync();`

For starting the message exchange, you can call two types of methods. If it is request response type where you only expect the result to be sent once, then use the following.

`var result = await socket.GetResponse<Data, Result>(inputData, result1 => result1.Id.Equals(inputData.Id));`

Note that the second parameter specifies a predicate to filter out the responses if the server is sending multiple messages over the same call. This depends on how the server is implemented and the filter may not always be needed.

If you expect a stream of response from the server, you can use the following:

```
socket.GetObservable<Data, Result>(inputData, result1 => result1.Id.Equals(inputData.Id))
                .Subscribe(resultReceived =>
                    {
                        Console.WriteLine($"Result received from server {resultReceived.Value}");
                    },
                    exception =>
                    {
                        Console.WriteLine($"Error: {exception.Message}");
                    });
```

To subscribe to the connection status and error :

```
socket.StatusStream.Subscribe(value =>
{
Console.WriteLine(value.ConnectionState != ConnectionState.Disconnected
? $"Status: {value.ConnectionState}; {value.Message}"
: $"Status: {value.ConnectionState}; {value.Error?.Message}");
});
```
