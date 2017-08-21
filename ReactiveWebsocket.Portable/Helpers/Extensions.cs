using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveWebsocket.Model;

namespace ReactiveWebsocket.Helpers
{
    public static class Extensions
    {
        public static IObservable<T> VerifyConnected<T>(this IObservable<T> source, Status status)
        {
            return Observable.Create<T>(observer =>
            {
                if (status.ConnectionState == ConnectionState.Connected) return source.Subscribe(observer);
                observer.OnError(NotconnectedError());
                return Disposable.Empty;
            });
        }

        public static Exception NotconnectedError()
        {
            return new InvalidOperationException("Not connected yet");
        }
    }
}
