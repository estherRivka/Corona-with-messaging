

using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace FinanceMinistryService
{
    public class PatientCreatedHandler : IHandleMessages<IPatientAdded>
    {
        static ILog log = LogManager.GetLogger<IPatientCreated>();
        public Task Handle(IPatientAdded message, IMessageHandlerContext context)
        {
            log.Info($"Received PatientCreated, PatientId = {message.PatientId}");
            //throw new Exception();
             return Task.CompletedTask;

        }
    }
}
