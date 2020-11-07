using System.Collections;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace AzureSamples.Storage.Blob
{
    public interface IApplicationArgs
    {
        bool UploadFiles { get; }
        string FilesDirectory { get; }
        IEnumerable<string> FileNames { get; }
    }

    public sealed class ProgramOptions : IApplicationArgs
    {
        [Option('e', "error-status-code", Required = false, Default = -1, HelpText = "Exit status code to assign to Environment.ExitCode if an Exception occurs, defaults to -1")]
        public int ErrorStatusCode { get; set; }

        [Option('p', "pause-before-exit", Default = false, Required = false, HelpText = "True to have console pause awaiting Enter key to be pressed before exiting")]
        public bool PauseBeforeExit { get; set; }
        
        [Option('u', "upload-files", Default = false, Required = false, HelpText = "True to invoke the operation to upload files to blob storage")]
        public bool UploadFiles { get; set; }

        [Option('f', "files-directory", Default = "uploads", Required = false, HelpText = "Path to where files exist to be uploaded to blob storage")]
        public string FilesDirectory { get; set; }
        
        [Option('n', "file-names", Separator = ',', Default = "", Required = false, HelpText = "Names of files in FilesDirectory to be uploaded separated by a comma, if 'all' is specified will upload all files in directory")]
        public IEnumerable<string> FileNames { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{nameof(ProgramOptions)} Provided:");
            sb.AppendLine("--------------------------------------------------------------------");
            sb.AppendLine($"  -u, --upload-files        : {UploadFiles}");
            sb.AppendLine($"  -f, --files-directory     : {FilesDirectory}");
            sb.AppendLine($"  -n, --file-names          : {FileNames}");
            sb.AppendLine($"  -e, --error-status-code   : {ErrorStatusCode}");
            sb.AppendLine($"  -p, --pause-before-exit   : {PauseBeforeExit}");
            sb.AppendLine("--------------------------------------------------------------------");

            return sb.ToString();
        }
    }
}
