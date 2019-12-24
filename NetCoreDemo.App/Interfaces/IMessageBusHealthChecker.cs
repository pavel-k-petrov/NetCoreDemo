namespace HttpRequestProcessing
{
    public interface IMessageBusHealthChecker
    {
        MessageBusHealthStatus GetStatus();
    }

    public enum MessageBusHealthStatus
    {
        Online,
        Offline
    }
}