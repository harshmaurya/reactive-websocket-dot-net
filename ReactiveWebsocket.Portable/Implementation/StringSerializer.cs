using System.Text;
using ReactiveWebsocket.Abstractions;
using System;

namespace ReactiveWebsocket.Implementation
{
    public class StringSerializer : ISerializer
    {
        public byte[] Serialize<TRequestType>(TRequestType payload)
        {
            if (payload is string)
            {
                return Encoding.UTF8.GetBytes((string)(object)payload);
            }

            throw new InvalidOperationException("Only string types allowed");
        }
    }
}
