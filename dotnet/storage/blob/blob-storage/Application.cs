using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AzureSamples.Storage.Blob
{
    public sealed class Application
    {
        private readonly IBlobStorageService _service;
        private readonly ILogger _logger;

        public Application(ILogger<Application> logger, IBlobStorageService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task RunAsync(IApplicationArgs args, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"{nameof(Application)}.{nameof(RunAsync)} - Start");

            if (args.UploadFiles)
                await UploadFilesAsync(args.FilesDirectory, args.FileNames, cancellationToken);

            _logger.LogInformation($"{nameof(Application)}.{nameof(RunAsync)} - End");
        }

        private async Task UploadFilesAsync(string filesDirectory, IEnumerable<string> fileNames, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"{nameof(Application)}.{nameof(UploadFilesAsync)} - Start");

            var uploadFileTasks = fileNames
                .Select(fileName => Path.Combine(filesDirectory, fileName))
                .Select(filePath => _service.UploadFileAsync(Path.GetFileName(filePath), filePath, cancellationToken));

            await Task.WhenAll(uploadFileTasks);

            _logger.LogInformation($"{nameof(Application)}.{nameof(UploadFilesAsync)} - End");
        }
    }
}
