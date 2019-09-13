using System.Text;
using Newtonsoft.Json;
using ReactiveWebsocket.Abstractions;

namespace ReactiveWebsocket.Implementation
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize<TRequestType>(TRequestType payload)
        {
            var result = JsonConvert.SerializeObject(payload);
            return Encoding.UTF8.GetBytes(result);
        }
    }
}