using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AzureSamples.Storage.Template
{
    public interface IAppServicesProvider
    {
        TService GetRequiredService<TService>();
        void Dispose();
    }

    internal sealed class AppServicesProvider : IAppServicesProvider, IDisposable
    {
        private AppServicesProvider()
        {
            BuildConfiguration();
            RegisterServices();
        }

        public static AppServicesProvider Build()
        {
            var appServiceProvider = new AppServicesProvider();
            return appServiceProvider;
        }

        private IConfigurationRoot _configuration;
        private IServiceProvider _serviceProvider;

        public TService GetRequiredService<TService>()
        {
            return _serviceProvider.GetRequiredService<TService>();
        }

        private void BuildConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT");

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", false)
               .AddJsonFile($"appsettings.{environmentName}.json", true)
               .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        private void RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddLogging((logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole(options => options.IncludeScopes = true);
                logging.AddDebug();
            });

            // services.AddOptions<SomeConfigClass>()
            //    .Bind(_configuration.GetSection("SomeConfigSection"));

            // TODO: add other services here
            // ...
            
            _serviceProvider = services.BuildServiceProvider();
        }

        private void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        public void Dispose()
        {
            DisposeServices();

            _serviceProvider = null;
            _configuration = null;
        }
    }
}
