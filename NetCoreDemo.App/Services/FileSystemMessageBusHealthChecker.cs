using System.IO;

namespace HttpRequestProcessing
{
    public class FileSystemMessageBusHealthChecker : IMessageBusHealthChecker
    {
        private bool _directoryExists;

        public FileSystemMessageBusHealthChecker(FileSystemMessageBusOptions options)
        {
            _directoryExists = Directory.Exists(options.StorageFolderPath);
        }

        public MessageBusHealthStatus GetStatus()
        {
            if (_directoryExists)
                return MessageBusHealthStatus.Online;
                
            return MessageBusHealthStatus.Offline;
        }
    }
}