using System.Text;
using System.Text.Json;

namespace JobApplicationTracker.Worker.Messaging.Serialization
{
    public sealed class JsonEventSerializer : IEventSerializer
    {
        private readonly JsonSerializerOptions _options;

        public JsonEventSerializer(JsonSerializerOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public byte[] Serialize<T>(T message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var json = JsonSerializer.Serialize(message, _options);
            return Encoding.UTF8.GetBytes(json);
        }

        public T? Deserialize<T>(ReadOnlyMemory<byte> body)
        {
            if (body.IsEmpty)
            {
                return default;
            }

            var json = Encoding.UTF8.GetString(body.Span);
            return JsonSerializer.Deserialize<T>(json, _options);
        }
    }
}