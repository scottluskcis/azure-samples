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

### Command Line

This project takes advantage of the [Command Line Parser Library for CLR and NetStandard](https://github.com/commandlineparser/commandline). If you run the project with a `--help` argument you will see all options available when running the console application.

You can either run this project via Visual Studio or via Command Line

#### Visual Studio

If you want to run this project via Visual Studio for debugging you will want to specify the command line arguments via 

1. Open the `blob-storage.csproj` in Visual Studio
2. Right click on the `blob-storage` project
3. Choose `Properties`
4. Click on `Debug`
5. In the `Application Arguments` section specify the startup arguments you want to run the application under

#### Command Line

If you want to run this project via command line you will want to first build the project and then `cd` to the output directory (i.e. `bin`) folder where the `blob-storage.exe` file will be created. 

When running via command line you will want to pass the appropriate arguments to run the console application under. For all possible options use `--help` for help

#### Examples

Here are a few examples of different scenarios (arguments) you might use to run the application.

##### Help

To see help information and all available arugments

```powershell
blob-storage --help
```

##### Upload, Download, then Delete

This scenario will upload a file from specified location, download it into specified location, and then delete it from blob storage. You can modify any of these arguments to perform 1 or more of the actions.

```powershell
blob-storage -u -f "uploads" -n "all" -d -o "downloads" -x
```

In the above scenario:

* `-u` indicates upload files
* `-f` indicates name of folder where files to be uploaded live
* `-n` indicates the name of specific file(s) to be uploaded, specify all to upload all files in specified folder
* `-d` indicates download the file that was uploaded to a specified directory
* `-o` indicates the folder to download files to
* `-x` indicates delete the file from blob storage when done

The above uses the `Short Name` for the arguments, but you can use the `Long Name` if you prefer. Again, use `--help` argument to see a printout of all arguments available

##### View Contents of Blob Storage

This scenario reads the names of blobs in storage and prints them out

```powershell
blob-storage -y
```

* `y` indicates only retrieve names of blobs in storage and print them out, no other action is performed

## References

* [Azure Blob storage documentation](https://docs.microsoft.com/en-us/azure/storage/blobs/)
* [Azure Storage Blobs client library for .NET](https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/storage/Azure.Storage.Blobs/README.md)
* [Azure Storage Emulator for development and testing](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* [Azure Storage Account](https://docs.microsoft.com/azure/storage/common/storage-account-overview)