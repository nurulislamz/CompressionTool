using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.CommandLine;

namespace CompressionTool
{
  public static class Program
  {
    public static int Main(string[] args)
    {

      var host = CreateHostBuilder(args).Build();
      using var scope = host.Services.CreateScope();
      var app = scope.ServiceProvider.GetRequiredService<App>();
      app.Run(args);

      return 0;
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      var builder = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
          services.AddSingleton<App>();
          services.AddTransient<IArgumentParser, ArgumentParser>();
          services.AddTransient<IFrequencyCounter, FrequencyCounter>();
          services.AddTransient<IPriorityQueue, PriorityQueue>();
        });

      return builder;
    }
  }
}