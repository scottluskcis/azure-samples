using System;

namespace AzureSamples.Storage.Blob
{
    public interface IBlobStorageServiceConfiguration
    {
        string ConnectionString { get; }
        string ContainerName { get; }
        void Validate();
    }

    public class BlobStorageServiceConfiguration : IBlobStorageServiceConfiguration
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }

        public void Validate()
        {
            if(string.IsNullOrEmpty(ConnectionString))
                throw new ArgumentException($"{nameof(ConnectionString)} was not found");

            if(string.IsNullOrEmpty(ContainerName))
                throw new ArgumentException($"{nameof(ContainerName)} was not found");
        }
    }
}