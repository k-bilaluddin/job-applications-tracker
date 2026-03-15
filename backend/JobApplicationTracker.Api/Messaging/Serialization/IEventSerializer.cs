namespace JobApplicationTracker.Api.Messaging.Serialization
{
    public interface IEventSerializer
    {
        byte[] Serialize<T>(T message);
        T? Deserialize<T>(ReadOnlyMemory<byte> body);
    }
}
