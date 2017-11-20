Welcome to project home for reactive websocket - A portable .Net library providing client side websocket API. 

# Overview
Web-sockets are the latest addition to the communication protocol which standardizes the way client communicates with the server. It provides full duplex communication channel over a single TCP connection which is great considering the workarounds that existed previously for achieving the same result.For .Net world, Microsoft has provided a client implementation of WebSocket which can connect to any server supporting the same protocol. However, it is at a much lower level leaving the developer to deal with a lot of boilerplate code, not to mention effort which goes into making the code robust.Reactive WebSocket is an open source library which has been developed as a wrapper of the Microsoft’s implementation providing the following functionalities:
Based on a portable library, supports desktop and Android client. Other platforms coming soon.
Built-in Rx support. Supports both request-response and streaming style of communication.
Supports simple string as well as custom message serialization/deserialization. JSON support coming next.
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

` Install-Package ReactiveWebsocket.Desktop -Version 1.0.0-preview00006`

**For Android client:**

`Install-Package ReactiveWebsocket.Android -Version 1.0.0-preview00006`

**For Ios client:**

`Install-Package ReactiveWebsocket.Ios -Version 1.0.0-preview00006`

Using the above libraries, it is also possible to use them in all types of Xamarin projects. The [sample](https://github.com/harshmaurya/reactive-websocket-samples) programs provided showcase the features of the library.

# Usage:

Other than the brief description here, the complete documentation and code samples can be found [here](https://github.com/harshmaurya/reactive-websocket-samples).

The API is very easy to use. Firstly, you should decide what is your communication and message type. Whether it is a string, json or your custom implementation. Also, is the message going to be always in a particular format or can it change based on the method call. Let’s say you have a very simple requirement and the server and client communicate using string objects only. You can initialize a StringWebsocketClient which will serve the purpose.
Initialization is as simple as calling the constructor.

`var socket = new StringWebsocketClient();`

After you have selected the right client, you need to connect to the server which supports WebSockets.

`var success = await socket.ConnectAsync(new Uri(Uri), CancellationToken.None);`

Then, based on type of API call you can call two types of methods. If it is request response type where you only expect the result to be sent once, then use the following.

var result = await socket.GetResponse(request, filter => true);

Note that the second parameter specifies a predicate to filter out the responses. Here we would get all the responses from the server even if it is a result of some previous method call. You should ideally have a mechanism in your application code to identify and correlate the results. For example setting an Id field in the request and using the same Id to populate the result object on server side.If you expect a stream of response from the server, you can use the following:

```
socket.GetObservable(request, filter => true)
.Subscribe(value =>
{
Console.WriteLine($"Message received from server {value}");
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
