using NServiceBus;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MagenDavidAdomService
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "MagenDavidAdom";

            var endpointConfiguration = new EndpointConfiguration("MagenDavidAdom");

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


            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(10));

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Delayed(
                customizations: delayed =>
                {
                    delayed.NumberOfRetries(2);
                    delayed.TimeIncrease(TimeSpan.FromMinutes(5));
                });

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

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
