
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace HealthMinistryService
{
    public class PatientCreatedHandler : IHandleMessages<IPatientCreated>
    {

        static ILog log = LogManager.GetLogger<IPatientCreated>();
        public Task Handle(IPatientCreated message, IMessageHandlerContext context)
        {
            log.Info($"Received PatientCreated, PatientId = {message.PatientId}");


            return context.Send<ISendEmail>(msg =>
            {
                msg.PatientId = message.PatientId;
            });


        }
    }
}
