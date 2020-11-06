using System;
using System.Threading.Tasks;

namespace AzureSamples.Storage.Blob
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using var app = new Application(args);
                await app.RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
