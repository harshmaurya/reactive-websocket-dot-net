using System;

namespace ReactiveWebsocket.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var stringExample = new JsonPayloadExample();
            stringExample.RunAsync();
            Console.Read();
        }
    }
}
