using System.Collections.Generic;
using CommandLine;

namespace AzureSamples.Storage.Blob
{
    /// <summary>
    /// settings that identify how to run the application when launched
    /// </summary>
    public interface IApplicationArgs
    {
        bool UploadFiles { get; }
        string UploadsDirectory { get; }
        IEnumerable<string> FilesToUpload { get; }
        bool DownloadBlobs { get; }
        string DownloadsDirectory { get; }
        bool DeleteBlobs { get; }
        bool DeleteContainer { get; }
        bool PrintExistingOnly { get; }
        bool GenerateSasTokens { get; }
    }

    public sealed class ProgramOptions : IApplicationArgs
    {
        [Option('e', "environment", Required = false, Default = "dev", HelpText = "Environment to run the application under (i.e. local, dev, staging, prod)")]
        public string Environment { get; set; }

        [Option('s', "error-status-code", Required = false, Default = -1, HelpText = "Exit status code to assign to Environment.ExitCode if an Exception occurs, defaults to -1")]
        public int ErrorStatusCode { get; set; }

        [Option('p', "pause-before-exit", Default = false, Required = false, HelpText = "True to have console pause awaiting Enter key to be pressed before exiting")]
        public bool PauseBeforeExit { get; set; }

        [Option('t', "timeout", Default = 120, Required = false, HelpText = "Value in seconds to wait before timing out and sending a cancellation of any async requests, defaults to 120")]
        public int Timeout { get; set; }

        [Option('u', "upload-files", Default = false, Required = false, HelpText = "True to invoke the operation to upload files to blob storage")]
        public bool UploadFiles { get; set; }

        [Option('f', "uploads-directory", Default = "uploads", Required = false, HelpText = "Path to where files exist to be uploaded to blob storage")]
        public string UploadsDirectory { get; set; }
        
        [Option('n', "upload-file-names", Separator = ',', Default = null, Required = false, HelpText = "Names of files in UploadsDirectory to be uploaded separated by a comma, if 'all' is specified will upload all files in directory")]
        public IEnumerable<string> FilesToUpload { get; set; }

        [Option('d', "download-blobs", Default = false, Required = false, HelpText = "True to invoke the operation to download files from blob storage")]
        public bool DownloadBlobs { get; set; }

        [Option('o', "downloads-directory", Default = "downloads", Required = false, HelpText = "Path to where files should be saved to when downloaded from blob storage")]
        public string DownloadsDirectory { get; set; }

        [Option('x', "delete-blobs", Default = false, Required = false, HelpText = "True to delete any blobs that exist in specified container")]
        public bool DeleteBlobs { get; set; }

        [Option('z', "delete-container", Default = false, Required = false, HelpText = "True to delete the entire blob container and all blobs that are saved, overrides DeleteBlobs options")]
        public bool DeleteContainer { get; set; }

        [Option('y', "print-only", Default = false, Required = false, HelpText = "True to only print names of existing blobs in storage, no other operations are performed if this is true")]
        public bool PrintExistingOnly { get; set; }

        [Option('k', "generate-sas-tokens", Default = false, Required = false, HelpText = "True to generate SAS tokens for blobs")]
        public bool GenerateSasTokens { get; set; }
    }
}
