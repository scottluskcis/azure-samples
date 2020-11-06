using System.Text;
using CommandLine;

namespace AzureSamples.Storage.Blob
{
    public interface IApplicationArgs
    {
    }

    public sealed class ProgramOptions : IApplicationArgs
    {
        [Option('e', "error-status-code", Required = false, Default = -1, HelpText = "Exit status code to assign to Environment.ExitCode if an Exception occurs, defaults to -1")]
        public int ErrorStatusCode { get; set; }

        [Option('p', "pause-before-exit", Default = false, Required = false, HelpText = "True to have console pause awaiting Enter key to be pressed before exiting")]
        public bool PauseBeforeExit { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{nameof(ProgramOptions)} Provided:");
            sb.AppendLine("--------------------------------------------------------------------");
            
            sb.AppendLine($"  -e, --error-status-code {ErrorStatusCode}");
            sb.AppendLine($"  -p, --pause-before-exit {PauseBeforeExit}");
            sb.AppendLine("--------------------------------------------------------------------");

            return sb.ToString();
        }
    }
}
