using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinanceMinistryService
{
    class NotifyQuarantineHandler : IHandleMessages<INotifyQuarantine>
    {
        static ILog log = LogManager.GetLogger<IPatientCreated>();


        public Task Handle(INotifyQuarantine message, IMessageHandlerContext context)
        {
            log.Info($"Received PatientCreated, PatientId aaa");


            return Task.CompletedTask;
        }
    }
}
