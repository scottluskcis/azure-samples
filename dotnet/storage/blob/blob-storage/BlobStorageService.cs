using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace AzureSamples.Storage.Blob
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string blobName, string filePath, CancellationToken cancellationToken = default);
        Task<string> UploadAsync(string blobName, Stream content, CancellationToken cancellationToken = default);
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
            _logger.LogInformation($"Generating unique name for Blob '{blobName}'");
            var uniqueName = $"{Guid.NewGuid()}-{blobName}";
            _logger.LogInformation($"Unique name '{uniqueName}' generated for Blob '{blobName}'");

            _logger.LogInformation($"Retrieving BlobClient for Blob '{uniqueName}'");
            var client = await GetBlobClientAsync(uniqueName);

            _logger.LogInformation($"Uploading Blob '{uniqueName}'");
            var response = await client.UploadAsync(content, cancellationToken);
            _logger.LogDebug($"Blob '{uniqueName}' Uploaded, BlobSequenceNumber: '{response.Value.BlobSequenceNumber}', VersionId: '{response.Value.VersionId}', ETag: '{response.Value.ETag}'");

            return uniqueName;
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