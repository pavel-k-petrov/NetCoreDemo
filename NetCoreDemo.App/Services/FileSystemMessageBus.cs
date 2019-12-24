using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HttpRequestProcessing
{
    public class FileSystemMessageBus : IRequestProcessor, IDisposable
    {
        private FileSystemWatcher _fileWatcher;
        private bool _isDisposed = false;

        public FileSystemMessageBus()
        {
            _fileWatcher = new FileSystemWatcher();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _fileWatcher.Dispose();
            _isDisposed = true;
        }

        public Task<ResponseModel> Process(RequestModel request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ResponseModel());
        }
    }
}