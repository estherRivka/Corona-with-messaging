using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HealthMinistryService
{
    public class SendEmailHandler : IHandleMessages<ISendEmail>
    {
        static Random random = new Random();

        static ILog log = LogManager.GetLogger<ISendEmail>();
        public Task Handle(ISendEmail message, IMessageHandlerContext context)
        {
            log.Info($"Received PatientCreated, PatientId = {message.PatientId}");

            return context.Publish<ISentEmail>(msg =>
            {
                msg.PatientId = message.PatientId;
            });


        }
    }
}
