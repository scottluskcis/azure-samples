using System.Text;
using CommandLine;

namespace AzureSamples.Storage.Template
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
    }
}
