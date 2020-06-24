
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace MagenDavidAdomService
{
    public class PatientCreatedHandler : IHandleMessages<IPatientCreated>
    {
        static ILog log = LogManager.GetLogger<IPatientCreated>();
        public  Task Handle(IPatientCreated message, IMessageHandlerContext context)
        {
            log.Info($"Received PatientCreated, PatientId = {message.PatientId}");


            return context.Send<INotifyQuarantine>(msg =>
              {
                  msg.Patient = new Messages.Patient { Id = message.PatientId };
                  msg.PatientId = message.PatientId;
              });



        }
    }
}
