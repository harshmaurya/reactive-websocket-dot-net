using System;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Widget;
using ReactiveWebsocket.Public;

namespace ReactiveWebsocket.Droid.Samples
{
    [Activity(Label = "ReactiveWebsocket.Android.Samples", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private const string Uri = "ws://reactivewebsocket.azurewebsites.net/ReactiveWebSocketServer/stringdemo";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var button = FindViewById<Button>(Resource.Id.buttonSubscribe);
            button.Click += delegate {
                Subscribe();
            };
        }

        private async void Subscribe()
        {
            var socket = new StringWebsocketClient();
            var success = await socket.ConnectAsync(new Uri(Uri), CancellationToken.None);
            if (!success) return;

            const string request = "test";
            socket.GetObservable(request, s => true)
                .Subscribe(message =>
                {
                    var txtView = FindViewById<TextView>(Resource.Id.txtResult);
                    txtView.Text = message;
                });
        }
    }
}

