
using Messages.Commands;
using Newtonsoft.Json;
using NServiceBus;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IsolationService
{
    class Program
    {
        static async Task Main()
        {
            const string endpointName = "Isolation";

            Console.Title = endpointName;

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            endpointConfiguration.EnableInstallers();

            endpointConfiguration.AuditProcessedMessagesTo("audit");

            endpointConfiguration.AuditSagaStateChanges(
                      serviceControlQueue: "Particular.Servicecontrol");

/*            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            serialization.Settings(settings);*/


            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

            var persistenceConnection = ConfigurationManager.ConnectionStrings["persistenceConnection"].ToString();

            var transportConnection = ConfigurationManager.ConnectionStrings["transportConnection"].ToString();


            persistence.SqlDialect<SqlDialect.MsSqlServer>();

            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(persistenceConnection);
                });

            var outboxSettings = endpointConfiguration.EnableOutbox();


            outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
            outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));


            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(10));

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(3);
                    delayed.TimeIncrease(TimeSpan.FromMinutes(3));
                });

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology()
                .ConnectionString(transportConnection);

            var routing = transport.Routing();

            routing.RouteToEndpoint(
                assembly: typeof(INotifyPolice).Assembly,
                destination: "Police");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
