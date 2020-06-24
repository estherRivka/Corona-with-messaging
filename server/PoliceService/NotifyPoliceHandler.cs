using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoliceService
{
    public class NotifyPoliceHandler : IHandleMessages<INotifyPolice>
    {
        static ILog log = LogManager.GetLogger<IPatientCreated>();

        public Task Handle(INotifyPolice message, IMessageHandlerContext context)
        {
            log.Info($"Received PatientCreated, PatientId = {message.PatientId}");
            return Task.CompletedTask;
        }
    }
}
