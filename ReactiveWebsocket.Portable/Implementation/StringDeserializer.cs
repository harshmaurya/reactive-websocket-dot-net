using System;
using System.Text;
using ReactiveWebsocket.Abstractions;

namespace ReactiveWebsocket.Implementation
{
    public class StringDeserializer : IDeserializer
    {
        public string DeserializePayload(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        
        public bool TryDeserialize<TResultType>(byte[] bytes, out TResultType result)
        {
            result = default;
            if (typeof(TResultType) == typeof(string))
            {
                try
                {
                    result = (TResultType)(object)DeserializePayload(bytes);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            throw new InvalidOperationException("Only string types allowed");
        }
    }
}