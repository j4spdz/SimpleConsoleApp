using System;
using System.IO;
using System.Xml.Schema;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using ConsoleUI.Helper;

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

            //var svc = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);
            //svc.Run();

            Directory.CreateDirectory("./output");

            XmlReader reader = XmlReader.Create(@"Misc\sample1.xml");
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference schema = new XmlSchemaInference();
            schemaSet = schema.InferSchema(reader);

            foreach (XmlSchema s in schemaSet.Schemas())
            {
                using (var stringWriter = new Utf8StringWriter())
                {
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        s.Write(writer);
                    }

                    var newXml = stringWriter.ToString().FormatXml();
                    File.WriteAllText(@"./output/sampleXsd.xml", newXml);
                    Console.WriteLine(newXml);
                }
            }

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
