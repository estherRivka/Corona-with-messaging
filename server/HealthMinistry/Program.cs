using CoronaApp.Api.Exceptions;
using NServiceBus;
using NServiceBus.Transport;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HealthMinistryService
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "HealthMinistry";

            var endpointConfiguration = new EndpointConfiguration("HealthMinistry");
            endpointConfiguration.EnableInstallers();

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();

            var connection = "Server=localhost\\MSSQLSERVER01; Database= coronaInformation; Trusted_Connection = True;";


            persistence.SqlDialect<SqlDialect.MsSqlServer>();

            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });


            var outboxSettings = endpointConfiguration.EnableOutbox();

            outboxSettings.KeepDeduplicationDataFor(TimeSpan.FromDays(6));
            outboxSettings.RunDeduplicationDataCleanupEvery(TimeSpan.FromMinutes(15));

            var recoverability = endpointConfiguration.Recoverability();
                     recoverability.CustomPolicy(MyCoronaServiceRetryPolicy);

            var transport = endpointConfiguration.UseTransport<LearningTransport>();


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
                return RecoverabilityAction.DelayedRetry(TimeSpan.FromSeconds(5));
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
