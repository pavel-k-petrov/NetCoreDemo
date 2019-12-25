using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HttpRequestProcessing
{
    public class FileSystemMessageBus : IRequestProcessor, IDisposable
    {
        private FileSystemWatcher _fileWatcher;
        private bool _isDisposed = false;
        private ConcurrentDictionary<string, TaskCompletionSource<ResponseModel>> _watchedTasks =
                new ConcurrentDictionary<string, TaskCompletionSource<ResponseModel>>();
        private readonly IRequestToFileNameMapper fileNameMapper;

        public FileSystemMessageBus(FileSystemMessageBusOptions options, IRequestToFileNameMapper fileNameMapper)
        {
            _fileWatcher = new FileSystemWatcher(options.StorageFolderPath);
            _fileWatcher.EnableRaisingEvents = false;
            _fileWatcher.Created += OnFileCreated;
            this.fileNameMapper = fileNameMapper;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {

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
            _fileWatcher.EnableRaisingEvents = false;
            return Task.FromResult(new ResponseModel());
        }
    }
}