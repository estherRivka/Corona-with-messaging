using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Serilog;
using Messages.Events;
using NServiceBus.Transport;
using CoronaApp.Api.Exceptions;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace CoronaApp.Api
{
    public class Program
    {

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();

         


        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                 .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();
            });

            try
            {
                Log.Information("The program has started!!!");

                CreateHostBuilder(args).Build().Run();


            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseNServiceBus(context =>
            {


                var endpointConfiguration = new EndpointConfiguration("CoronaApp");

                endpointConfiguration.EnableInstallers();

                endpointConfiguration.AuditProcessedMessagesTo("audit");

                endpointConfiguration.AuditSagaStateChanges(
                        serviceControlQueue: "Particular.Servicecontrol");

/*                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                };
                var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
                serialization.Settings(settings);*/


                var recoverability = endpointConfiguration.Recoverability();

                var outboxSettings = endpointConfiguration.EnableOutbox();

                outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
                outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));



                recoverability.Delayed(
                    customizations: delayed =>
                    {
                        delayed.NumberOfRetries(2);
                        delayed.TimeIncrease(TimeSpan.FromMinutes(4));
                    });

                recoverability.Immediate(
                    customizations: immidiate =>
                   {
                       immidiate.NumberOfRetries(3);

                   });

                var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
                transport.UseConventionalRoutingTopology()
                    .ConnectionString("host= localhost:5672");

                
                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                var connection = Configuration.GetConnectionString("NServiceBusPersitence");

                persistence.SqlDialect<SqlDialect.MsSqlServer>();

                persistence.ConnectionBuilder(
                    connectionBuilder: () =>
                    {
                        return new SqlConnection(connection);
                    });

                var subscriptions = persistence.SubscriptionSettings();
                subscriptions.CacheFor(TimeSpan.FromMinutes(10));

                var conventions = endpointConfiguration.Conventions();
                conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
                conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

                return endpointConfiguration;
            })
            .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>()
                           .UseConfiguration(Configuration)
                           .UseSerilog();

             });




        


    }
}



