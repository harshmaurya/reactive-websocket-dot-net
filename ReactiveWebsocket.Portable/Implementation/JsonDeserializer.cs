using Newtonsoft.Json;
using ReactiveWebsocket.Abstractions;

namespace ReactiveWebsocket.Implementation
{
    public class JsonDeserializer : IDeserializer
    {
        public bool TryDeserialize<TResultType>(byte[] bytes, out TResultType result)
        {
            var jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            var success = true;
            var deserialized = JsonConvert.DeserializeObject<TResultType>(jsonString,
                new JsonSerializerSettings
                {
                    Error = (sender, args) =>
                    {
                        args.ErrorContext.Handled = true;
                        success = false;
                    }
                });
            result = deserialized;
            return success;
        }
    }
}