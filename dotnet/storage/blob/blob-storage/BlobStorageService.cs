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

        public async Task UploadFileAsync(string blobName, string filePath, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Reading file '{filePath}' into stream");
            await using var content = File.OpenRead(filePath);
            await UploadAsync(blobName, content, cancellationToken);
        }

        public async Task UploadAsync(string blobName, Stream content, CancellationToken cancellationToken = default)
        {
            var container = GetBlobContainerClient();

            _logger.LogInformation($"Uploading Blob '{blobName}'");
            var response = await container.UploadBlobAsync(blobName, content, cancellationToken);
            _logger.LogDebug($"Blob '{blobName}' Uploaded, BlobSequenceNumber: '{response.Value.BlobSequenceNumber}', VersionId: '{response.Value.VersionId}', ETag: '{response.Value.ETag}'");
        }

        private BlobClient GetBlobClient(string blobName)
        {
            if(string.IsNullOrEmpty(blobName))
                throw new ArgumentNullException(nameof(blobName));
            
            var container = GetBlobContainerClient();
            var blob = container.GetBlobClient(blobName);
            return blob;
        }

        private BlobContainerClient GetBlobContainerClient()
        {
            if(_configuration == null)
                throw new InvalidOperationException($"no {nameof(IBlobStorageServiceConfiguration)} was found, cannot create container");
            
            _configuration.Validate();

            var container = new BlobContainerClient(
                _configuration.ConnectionString,
                _configuration.ContainerName);

            container.Create();

            return container;
        }
    }
}