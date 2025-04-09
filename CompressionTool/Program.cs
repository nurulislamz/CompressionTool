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
      var host = CreateHostBuilder(args).Build();
      using var scope = host.Services.CreateScope();
      var compressionTool = scope.ServiceProvider.GetRequiredService<ICompressor>();
      compressionTool.Run(args[0]);

      return 0;
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
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

      return builder;
    }
  }
}