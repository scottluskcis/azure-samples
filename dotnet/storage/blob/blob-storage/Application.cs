using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            _logger.LogDebug($"{nameof(Application)}.{nameof(RunAsync)} - Start");

            try
            {
                IList<string> blobs = null;

                if (args.PrintExistingOnly)
                {
                    await GetBlobNamesAsync(cancellationToken);
                }
                else
                {
                    if (args.UploadFiles)
                        blobs = await UploadFilesAsync(args.UploadsDirectory, args.FilesToUpload?.ToList(), cancellationToken);

                    if (args.DownloadBlobs)
                        await DownloadBlobsAsync(args.DownloadsDirectory, blobs, cancellationToken);

                    if (args.DeleteContainer)
                        await DeleteContainerAsync(cancellationToken);
                    else if (args.DeleteBlobs)
                        await DeleteAsync(blobs, cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred {nameof(Application)}.{nameof(RunAsync)}");
                throw;
            }

            _logger.LogDebug($"{nameof(Application)}.{nameof(RunAsync)} - End");
        }

        private async Task<IList<string>> UploadFilesAsync(string filesDirectory, IList<string> fileNames, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"{nameof(Application)}.{nameof(UploadFilesAsync)} - Start");

            if (!(fileNames?.Any() ?? false))
            {
                _logger.LogWarning("No file names were specified to be uploaded, there is nothing to upload");
                return null;
            }

            EnsureDirectory(filesDirectory);

            IList<string> filesToUpload;
            if (fileNames.Count() == 1 && string.Equals(fileNames.ElementAt(0), "all", StringComparison.OrdinalIgnoreCase))
                filesToUpload = Directory.GetFiles(filesDirectory);
            else
                filesToUpload = fileNames.Select(file => Path.Combine(filesDirectory, file)).ToList();

            var uploadedFiles = new List<string>();

            var uploadFileTasks = filesToUpload
                .Select(filePath => _service.UploadFileAsync(Path.GetFileName(filePath), filePath, cancellationToken)
                    .ContinueWith(task =>
                    {
                        if (task.Exception != null)
                        {
                            _logger.LogError(task.Exception, $"Error uploading '{filePath}'");
                            throw task.Exception;
                        }

                        uploadedFiles.Add(task.Result);
                    }, cancellationToken));

            await Task.WhenAll(uploadFileTasks);

            _logger.LogDebug($"{nameof(Application)}.{nameof(UploadFilesAsync)} - End");

            return uploadedFiles;
        }

        private async Task<IEnumerable<string>> GetBlobNamesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"{nameof(Application)}.{nameof(GetBlobNamesAsync)} - Start");

            var blobs = (await _service.GetNamesAsync(cancellationToken)).ToList();

            var sb = new StringBuilder();
            sb.AppendLine($"'{blobs.Count}' blobs were found in the container");
            foreach (var name in blobs)
                sb.AppendLine(name);

            _logger.LogInformation(sb.ToString());

            _logger.LogDebug($"{nameof(Application)}.{nameof(GetBlobNamesAsync)} - End");

            return blobs;
        }

        private async Task DownloadBlobsAsync(string downloadsDirectory, IList<string> blobNames, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"{nameof(Application)}.{nameof(DownloadBlobsAsync)} - Start");

            if (!(blobNames?.Any() ?? false))
            {
                _logger.LogWarning("No blob names were specified to be downloaded, there is nothing to download");
                return;
            }

            EnsureDirectory(downloadsDirectory);

            IList<string> blobsToDownload;
            if (blobNames.Count() == 1 && string.Equals(blobNames.ElementAt(0), "all", StringComparison.OrdinalIgnoreCase))
                blobsToDownload = (await GetBlobNamesAsync(cancellationToken)).ToList();
            else
                blobsToDownload = blobNames.ToList();

            var downloadBlobTasks = blobsToDownload
                .Select(blobName => _service.DownloadAsync(blobName, Path.Combine(downloadsDirectory, blobName), cancellationToken));

            await Task.WhenAll(downloadBlobTasks);

            _logger.LogDebug($"{nameof(Application)}.{nameof(DownloadBlobsAsync)} - End");
        }

        private async Task DeleteAsync(IList<string> blobNames, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"{nameof(Application)}.{nameof(DeleteAsync)} - Start");

            if (!(blobNames?.Any() ?? false))
            {
                _logger.LogWarning("No blob names were specified to be deleted, there is nothing to delete");
                return;
            }

            var deleteBlobTasks = blobNames
                .Select(blobName => _service.DeleteAsync(blobName, cancellationToken));

            await Task.WhenAll(deleteBlobTasks);

            _logger.LogDebug($"{nameof(Application)}.{nameof(DeleteAsync)} - End");
        }

        private async Task DeleteContainerAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"{nameof(Application)}.{nameof(DeleteContainerAsync)} - Start");

            await _service.DeleteContainerAsync(cancellationToken);

            _logger.LogDebug($"{nameof(Application)}.{nameof(DeleteContainerAsync)} - End");
        }

        private void EnsureDirectory(string directory)
        {
            _logger.LogDebug($"Checking if directory '{directory}' exists");
            if (!Directory.Exists(directory))
            {
                _logger.LogInformation($"Creating directory '{directory}'");
                Directory.CreateDirectory(directory);
                _logger.LogInformation($"Directory '{directory}' created");
            }
            else
            {
                _logger.LogInformation($"Directory '{directory}' already exists");
            }
        }
    }
}
