# Blob Storage

## Overview

Refer to [Azure Blob storage documentation](https://docs.microsoft.com/en-us/azure/storage/blobs/) for details. This sample started from the details in the Microsoft documentation. Specifically, for setting up this sample the guidance at [Azure Storage Blobs client library for .NET](https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/storage/Azure.Storage.Blobs/README.md) was used.

Be sure to be familiar with the [Key concepts](https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/storage/Azure.Storage.Blobs/README.md#key-concepts) that Blob storage is designed for.

## Running the Sample

### Prerequisites

You will need an Azure [Storage Account](https://docs.microsoft.com/azure/storage/common/storage-account-overview) to use for testing out upload/download of blob storage. You can either create one in an [Azure subscription](https://azure.microsoft.com/free/) or you can use local storage as well.

To create a new Storage Account in Azure using Azure CLI run the following command using Azure CLI (replace any values encloded in `< >` with your own):

```powershell
az storage account create --name <account_name> --resource-group <resource_group> --location <location> --sku <sku>
```

To get the connection string of an existing storage account use the following command in Azure CLI  (replace any values encloded in `< >` with your own):

```powershell
az storage account show-connection-string --name <account_name> --resource-group <resource_group>
```

Alternatively if you want to configure and set it up you can use the [Azure Storage Emulator for development and testing](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) if you want to run this locally

### Configuration

Add a new file called `appsettings.dev.json` to the same directory as the `appsettings.json` and add the content below to the file. Update the information for `BlobStorage` to match the container storage account you setup for the [Prerequisites](#prerequisites)

```json
{
  "BlobStorage": {
    "ConnectionString": "<your_connection_string_here>",
    "ContainerName": "<your_container_name_here>"
  }
}
```

## References

* [Azure Blob storage documentation](https://docs.microsoft.com/en-us/azure/storage/blobs/)
* [Azure Storage Blobs client library for .NET](https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/storage/Azure.Storage.Blobs/README.md)
* [Azure Storage Emulator for development and testing](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* [Azure Storage Account](https://docs.microsoft.com/azure/storage/common/storage-account-overview)