
using CoronaApp.Api.Exceptions;
using Messages.Commands;
using Messages.Events;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Transport;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HealthMinistryService
{
    class Program
    {
        static async Task Main()
        {
            const string endpointName = "HealthMinistry";

            Console.Title = endpointName;

            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.EnableInstallers();

            endpointConfiguration.AuditProcessedMessagesTo("audit");

            endpointConfiguration.AuditSagaStateChanges(
                   serviceControlQueue: "Particular.Servicecontrol");

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
            var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            serialization.Settings(settings);


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

            var recoverability = endpointConfiguration.Recoverability();
                     recoverability.CustomPolicy(MyCoronaServiceRetryPolicy);

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology()
                .ConnectionString(transportConnection);


            var routing = transport.Routing();
            routing.RouteToEndpoint(
                    typeof(ISendEmail).Assembly, "HealthMinistry");




            RecoverabilityAction MyCoronaServiceRetryPolicy(RecoverabilityConfig config, ErrorContext context)
            {
                var action = DefaultRecoverabilityPolicy.Invoke(config, context);

                if (!(action is DelayedRetry delayedRetryAction))
                {
                    return action;
                }
                if (context.Exception is PatientNotExistExcption)
                {
                    return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                }
                // Override default delivery delay.
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromMinutes(3));
            }

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
