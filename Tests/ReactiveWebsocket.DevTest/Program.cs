using System;

namespace ReactiveWebsocket.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            //var stringExample = new StringPayloadExample();
            //stringExample.RunAsync();

            var jsonExample = new JsonPayloadExample();
            jsonExample.RunAsync();
            Console.Read();
        }
    }
}
