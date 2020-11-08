using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace AzureSamples.Storage.Blob
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string blobName, string filePath, CancellationToken cancellationToken = default);
        Task<string> UploadAsync(string blobName, Stream content, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> GetNamesAsync(CancellationToken cancellationToken = default);
        Task DownloadAsync(string blobName, string downloadPath, CancellationToken cancellationToken = default);
        Task DownloadAsync(string blobName, Stream destination, CancellationToken cancellationToken = default);
        Task DeleteAsync(string blobName, CancellationToken cancellationToken = default);
        Task DeleteContainerAsync(CancellationToken cancellationToken = default);
    }

    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobStorageServiceConfiguration _configuration;
        private readonly ILogger _logger;

        public BlobStorageService(
            IBlobStorageServiceConfiguration configuration,
            ILogger<BlobStorageService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(string blobName, string filePath, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Reading file '{filePath}' into stream");
            await using var content = File.OpenRead(filePath);
            var result = await UploadAsync(blobName, content, cancellationToken);
            return result;
        }

        public async Task<string> UploadAsync(string blobName, Stream content, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Generating unique name for Blob '{blobName}'");
            var uniqueName = $"{Guid.NewGuid()}-{blobName}";
            _logger.LogDebug($"Unique name '{uniqueName}' generated for Blob '{blobName}'");

            _logger.LogDebug($"Retrieving BlobClient for Blob '{uniqueName}'");
            var client = await GetBlobClientAsync(uniqueName);

            _logger.LogInformation($"Uploading Blob '{uniqueName}'");
            var response = await client.UploadAsync(content, cancellationToken);
            _logger.LogDebug($"Blob '{uniqueName}' Uploaded, BlobSequenceNumber: '{response.Value.BlobSequenceNumber}', VersionId: '{response.Value.VersionId}', ETag: '{response.Value.ETag}'");

            return uniqueName;
        }

        public async Task<IEnumerable<string>> GetNamesAsync(CancellationToken cancellationToken = default)
        {
            var items = await GetAsync(cancellationToken);
            var names = items.Select(s => s.Name);
            return names;
        }

        public async Task<IEnumerable<BlobItem>> GetAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Retrieve Blob Container Client");
            var client = await GetBlobContainerClientAsync();

            var items = new List<BlobItem>();

            _logger.LogInformation($"Reading Blobs in Container");
            await foreach (var item in client.GetBlobsAsync(cancellationToken: cancellationToken))
                items.Add(item);

            return items;
        }

        public async Task DownloadAsync(string blobName, string downloadPath, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Creating FileStream for path '{downloadPath}' to save downloaded blob '{blobName}' to");
            await using var file = File.OpenWrite(downloadPath);
            await DownloadAsync(blobName, file, cancellationToken);
        }

        public async Task DownloadAsync(string blobName, Stream destination, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Retrieving BlobClient for Blob '{blobName}'");
            var client = await GetBlobClientAsync(blobName);

            _logger.LogInformation($"Downloading blob '{blobName}'");
            BlobDownloadInfo download = await client.DownloadAsync(cancellationToken);

            _logger.LogInformation($"Copying downloaded blob '{blobName}' to destination");
            await download.Content.CopyToAsync(destination, cancellationToken);
        }

        public async Task DeleteAsync(string blobName, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Retrieving BlobClient for Blob '{blobName}'");
            var client = await GetBlobClientAsync(blobName);

            _logger.LogInformation($"Deleting blob '{blobName}'");
            await client.DeleteAsync(cancellationToken: cancellationToken);
        }

        public async Task DeleteContainerAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Retrieving Blob Container Client");
            var client = await GetBlobContainerClientAsync();

            _logger.LogInformation("Deleting Blob Container");
            await client.DeleteAsync(cancellationToken: cancellationToken);
        }

        private async Task<BlobClient> GetBlobClientAsync(string blobName)
        {
            if(string.IsNullOrEmpty(blobName))
                throw new ArgumentNullException(nameof(blobName));
            
            var container = await GetBlobContainerClientAsync();
            var blob = container.GetBlobClient(blobName);
            return blob;
        }

        private async Task<BlobContainerClient> GetBlobContainerClientAsync()
        {
            if(_configuration == null)
                throw new InvalidOperationException($"no {nameof(IBlobStorageServiceConfiguration)} was found, cannot create container");
            
            _configuration.Validate();

            var container = new BlobContainerClient(
                _configuration.ConnectionString,
                _configuration.ContainerName);

            if(_configuration.CreateContainerIfNotExists)
                await container.CreateIfNotExistsAsync();
;
            return container;
        }
    }
}