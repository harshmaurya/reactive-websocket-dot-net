using System;
using System.Text;
using ReactiveWebsocket.Implementation;

namespace ReactiveWebsocket.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var stringExample = new StringPayloadExample();
            stringExample.RunAsync();
            Console.Read();
        }
    }
}
