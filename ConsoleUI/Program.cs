using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ConsoleUI
{
  partial class Program
  {
    static void Main(string[] args)
    {
      var builder = new ConfigurationBuilder();
      BuildConfig(builder);

      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Build())
        .Enrich.FromLogContext()
        .CreateLogger();

      Serilog.Debugging.SelfLog.Enable(Console.Error);

      Log.Logger.Information("Application Starting");

      var host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
          services.AddTransient<IGreetingService, GreetingService>();
        })
        .UseSerilog()
        .Build();

      var svc = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);
      svc.Run();

      Console.ReadLine();
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
      builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
    }
  }
}
