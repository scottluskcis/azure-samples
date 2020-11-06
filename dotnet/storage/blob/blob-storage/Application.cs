﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AzureSamples.Storage.Blob
{
    public sealed class Application
    {
        private readonly ILogger _logger;

        public Application(ILogger<Application> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync(IApplicationArgs args)
        {
            _logger.LogInformation($"{nameof(Application)}.{nameof(RunAsync)} - Start");

            //var service = _provider.GetRequiredService<IBlobStorageService>();
            await Task.Delay(1);
            _logger.LogInformation($"{nameof(Application)}.{nameof(RunAsync)} - End");
        }
    }
}
