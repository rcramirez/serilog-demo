using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IO;

namespace logging_demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = CreateConfiguration();

            var loggingConfig = new LoggerConfiguration();
            loggingConfig
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithExceptionDetails()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("AspNet", LogEventLevel.Warning)
                    .MinimumLevel.Override("EdjCase", LogEventLevel.Warning)
                    .MinimumLevel.Override("MassTransit", LogEventLevel.Warning)
                    .MinimumLevel.Debug()
                    .WriteTo.Console();

            string elkUrl = configuration.GetValue<string>("ElkSinkUrl");
            string elkUsername = configuration.GetValue<string>("ElkSinkUsername");
            string elkPassword = configuration.GetValue<string>("ElkSinkPassword");

            loggingConfig
                    .WriteTo
                        .Elasticsearch(new ElasticsearchSinkOptions(new Uri(elkUrl))
                        {
                            IndexFormat = "log-demo-test" + "-{0:yyyy.MM.dd}",
                            AutoRegisterTemplate = true,
                            ModifyConnectionSettings = x => x.BasicAuthentication(elkUsername, elkPassword)
                        });

            Log.Logger = loggingConfig.CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder
                .SetBasePath(Directory.GetCurrentDirectory());


            builder
                .AddUserSecrets<Startup>();

            IConfiguration config = builder.Build();
            return config;
        }
    }
}
