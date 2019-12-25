namespace HttpRequestProcessing
{
    public interface IRequestToFileNameMapper
    {
        string GetFileNameForRequest(RequestModel request);
    }
}