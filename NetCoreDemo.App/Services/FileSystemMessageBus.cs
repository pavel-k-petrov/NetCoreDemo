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
        private ConcurrentDictionary<string, TaskCompletionSource<object>> _watchedTasks =
                new ConcurrentDictionary<string, TaskCompletionSource<object>>();
        private readonly IRequestToFileNameMapper _fileNameMapper;

        public FileSystemMessageBus(FileSystemMessageBusOptions options, IRequestToFileNameMapper fileNameMapper)
        {
            _fileWatcher = new FileSystemWatcher(options.StorageFolderPath);
            _fileWatcher.EnableRaisingEvents = true;
            _fileWatcher.Created += OnFileCreated;
            this._fileNameMapper = fileNameMapper;
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

        public async Task<ResponseModel> Process(RequestModel request, CancellationToken cancellationToken)
        {
            
            var taskCompletionSource = new TaskCompletionSource<object>();
            using (var cancellationRegistration = cancellationToken.Register(() => taskCompletionSource.SetCanceled()))
            {
                var fileName = _fileNameMapper.GetFileNameForRequest(request);
                //TODO Нужно учесть ситуацию, когда уже ожидается ответ на совпадающий запрос
                //варианты - регистрировать всех клиентов на уровне события и в событии вычитывать файл и уведомлять все TCS-ы
                //в2 - регистрировать один CTS на файл и ещё один на каждого клиента для возможности отработать внешний Cancel
                _watchedTasks.AddOrUpdate()
                await taskCompletionSource.Task;

                PutRequestIntoFile(fileName, request);
                return Task.FromResult(new ResponseModel());
            }
        }
    }
}