using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CompressionTool
{
  public static class Program
  {
    public static int Main(string[] args)
    {
      var builder = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
          services.AddSingleton<IFileOpener, FileOpener>();
          services.AddSingleton<IParser, Parser>();
        })
        .ConfigureAppConfiguration((hostContext, config) =>
        {
          config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
        });

      var host = builder.Build();

      // Run the host
      host.RunAsync();

      return 0;
    }
  }
}