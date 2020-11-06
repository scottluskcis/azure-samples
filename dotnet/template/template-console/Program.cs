using System;
using System.Threading.Tasks;
using CommandLine;

namespace AzureSamples.Storage.Template
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<ProgramOptions>(args)
                .MapResult(RunAsync, _ => Task.FromResult(1));
        }

        static async Task<int> RunAsync(ProgramOptions opts)
        {
            int exitCode = 0;

            IAppServicesProvider provider = null;
            try
            {
                Console.WriteLine(opts.ToString());

                Console.WriteLine("Executing...");

                provider = AppServicesProvider.Build();

                var service = provider.GetRequiredService<Application>();
                await service.RunAsync(opts);

                if (opts.PauseBeforeExit)
                {
                    Console.WriteLine("Execution Finished, Press any key to quit...");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                exitCode = opts.ErrorStatusCode;
                Console.WriteLine(ex.ToString());

                if (opts.PauseBeforeExit)
                    Console.ReadKey();
            }
            finally
            {
                provider?.Dispose();
                Environment.ExitCode = exitCode;
            }

            return exitCode;
        }
    }
}
