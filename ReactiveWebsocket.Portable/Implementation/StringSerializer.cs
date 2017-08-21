using System.Text;
using ReactiveWebsocket.Abstractions;

namespace ReactiveWebsocket.Implementation
{
    public class StringSerializer : ISingleMessageTypeSerializer<string>
    {
        public byte[] Serialize(string payload)
        {
            return Encoding.UTF8.GetBytes(payload);
        }
    }
}
