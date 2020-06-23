
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

            var options = new SendOptions();
            options.RequireImmediateDispatch();
            return context.Send<INotifyQuarantine>(msg =>
             {
                 msg.Patient = new Messages.Patient { Id = message.PatientId };
             }, options);



           // throw new Exception();

            /*            context.Send<ISendEmail>(msg =>
                         {
                             msg.Id = message.PatientId;
                         }).ConfigureAwait(false);*/



        }
    }
}
