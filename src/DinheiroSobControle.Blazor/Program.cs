using Castle.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Serilog.Sinks.OpenTelemetry;
using System.Net.Http;
using Microsoft.Extensions.Hosting.Internal;
using Volo.Abp;

namespace DinheiroSobControle.Blazor;

public class OpenObserveHostOptions
{
    public string AccessKey { get; set; }

    public string Host { get; set; } = "https://monitoria.devcorehub.com.br";

    public string LogsEndpoint
    {
        get
        {
            return $"{Host}/api/default";
        }
    }

    public Dictionary<string, object> ResourceAttributes { get; set; }


    public string AuthorizationHeader()
    {
        return $"Basic {AccessKey}";
    }

    public OtlpProtocol Protocol { get; set; } = OtlpProtocol.HttpProtobuf;

    public int BatchSizeLimit = 700;
    public int BufferingTimeLimit = 700;
    public int QueueLimit = 700;
}

public class Program
{
    public async static Task<int> Main(string[] args)
    {

        var assemblyName = typeof(Program).Assembly.GetName().Name;
        
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? Environments.Production;

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{env}.json")
            .Build();

        var ooOptions = config.GetSection("OpenObserve")
            .Get<OpenObserveHostOptions>();// Get<OpenObserveHostOptions>();

        ooOptions.ResourceAttributes["environment"] = env;

        if (!ooOptions.ResourceAttributes.ContainsKey("service_name"))
            ooOptions.ResourceAttributes["service_name"] = assemblyName;

        if (!ooOptions.ResourceAttributes.ContainsKey("service_version"))
            ooOptions.ResourceAttributes["service_version"] = "1.0";

        //new LoggerConfiguration()
        //        .WriteTo.Async(c => c.File("Log/bootstrap-logs.txt"))
        //        .WriteTo.Async(c => c.Console())

        //        //TODO: Move to appsettings.json
        //        .WriteTo.OpenTelemetry(otOptions =>
        //        {
        //            otOptions.LogsEndpoint = ooOptions.LogsEndpoint;
        //            otOptions.Protocol = ooOptions.Protocol;
        //            otOptions.HttpMessageHandler = new SocketsHttpHandler
        //            {
        //                ActivityHeadersPropagator = null
        //            };
        //            otOptions.IncludedData =
        //                IncludedData.SpanIdField
        //                | IncludedData.TraceIdField
        //                | IncludedData.MessageTemplateTextAttribute
        //                | IncludedData.MessageTemplateMD5HashAttribute;
        //            otOptions.Headers = new Dictionary<string, string>
        //            {
        //                ["Authorization"] = ooOptions.AuthorizationHeader(),
        //                ["stream-name"] = "integracaoAALP"
        //                //stream-name: default
        //            };
        //            otOptions.ResourceAttributes = ooOptions.ResourceAttributes;
        //            otOptions.BatchingOptions.BatchSizeLimit = 70;
        //            otOptions.BatchingOptions.BufferingTimeLimit = TimeSpan.FromSeconds(5);
        //            otOptions.BatchingOptions.QueueLimit = 10;
        //        })
        //        .CreateBootstrapLogger();

        //Log.Logger = new LoggerConfiguration()
        //    .Enrich.FromLogContext()

        //    #if DEBUG
        //    .MinimumLevel.Debug()
        //    #else
        //    .MinimumLevel.Information()
        //    #endif

        //    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        //    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        //    .Enrich.FromLogContext()
        //    .WriteTo.Async(c => c.File("Log/logs.txt"))
        //    .WriteTo.Async(c => c.Console())
        //    .WriteTo.PostgreSQL(
        //        connectionString: configuration.GetConnectionString("Default"),
        //        tableName: "Logs", 
        //        needAutoCreateTable: false,
        //        restrictedToMinimumLevel: LogEventLevel.Information
        //    )
        //    .CreateLogger();

        try
        {
            Log.Information($"Starting {assemblyName}.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    loggerConfiguration
#if DEBUG
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Debug)
#else
                        .MinimumLevel.Information()
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
#endif
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                        .MinimumLevel.Override("Swashbuckle.AspNetCore", LogEventLevel.Verbose)
                        .Enrich.FromLogContext()
                        .WriteTo.Async(c => c.File("Log/application-logs.txt"))
                        .WriteTo.Async(c => c.Console())

                        //TODO: Move to appsettings.json
                        .WriteTo.OpenTelemetry(otOptions =>
                        {
                            otOptions.Endpoint = ooOptions.LogsEndpoint;
                            otOptions.Protocol = OtlpProtocol.HttpProtobuf;
                            otOptions.HttpMessageHandler = new SocketsHttpHandler
                            {
                                ActivityHeadersPropagator = null
                            };
                            otOptions.IncludedData =
                                IncludedData.SpanIdField
                                | IncludedData.TraceIdField
                                | IncludedData.MessageTemplateTextAttribute
                                | IncludedData.MessageTemplateMD5HashAttribute;
                            otOptions.Headers = new Dictionary<string, string>
                            {
                                ["Authorization"] = ooOptions.AuthorizationHeader(),
                                ["stream-name"] = "dinheiroSobControle"
                            };
                            otOptions.ResourceAttributes = ooOptions.ResourceAttributes;
                            otOptions.BatchingOptions.BatchSizeLimit = 70;
                            otOptions.BatchingOptions.BufferingTimeLimit = TimeSpan.FromSeconds(5);
                            otOptions.BatchingOptions.QueueLimit = 10;
                        });
                });
            await builder.AddApplicationAsync<DinheiroSobControleBlazorModule>();

            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
