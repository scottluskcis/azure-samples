using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AzureSamples.Storage.Blob
{
    public sealed class Application : IDisposable
    {
        private readonly string[] _args;
        private readonly ILogger _logger;
        private readonly IAppServicesProvider _provider;

        public Application(string[] args)
        {
            _args = args;
            _provider = AppServicesProvider.Build();
            _logger = _provider.GetRequiredService<ILogger<Application>>();
        }

        public async Task RunAsync()
        {
            _logger.LogInformation($"{nameof(Application)}.{nameof(RunAsync)} - Start");
            await Task.Delay(1);
            _logger.LogInformation($"{nameof(Application)}.{nameof(RunAsync)} - End");
        }

        public void Dispose()
        {
            _provider?.Dispose();
        }
    }
}
