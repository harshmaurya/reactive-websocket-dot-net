using System.Text;
using ReactiveWebsocket.Abstractions;

namespace ReactiveWebsocket.Implementation
{
    public class StringDeserializer : ISingleMessageTypeDeserializer<string>
    {
        public string DeserializePayload(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        

        public bool TryDeserialize(byte[] bytes, out string result)
        {
            result = string.Empty;
            try
            {
                result = DeserializePayload(bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}