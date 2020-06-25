using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Messages.Commands;
using Messages.Events;
using NServiceBus.Logging;
using Microsoft.VisualBasic;
using System.Threading;

namespace IsolationService
{
    public class IsolationPolicy : Saga<IsolationDataPolicy>,
        IAmStartedByMessages<INotifiedQuarantine>,
        IAmStartedByMessages<ISentEmail>,
         IHandleTimeouts<QuarantineOver>
    {
        static ILog log = LogManager.GetLogger<IPatientCreated>();

        public async Task Handle(ISentEmail message, IMessageHandlerContext context)
        {
            log.Info($"Email sent, PatientId = {message.PatientId}");
            Data.IsSentEmail = true;
            Thread.Sleep(10000);

            await NotifyPolice(context);
        }

        public Task Handle(INotifiedQuarantine message, IMessageHandlerContext context)
        {
            log.Info($"notified quarantine, PatientId = {message.PatientId}");
            Data.IsNotifiedQuarantine = true;
     
            return NotifyPolice(context);

        }

        public async Task Timeout(QuarantineOver state, IMessageHandlerContext context)
        {
            log.Info("Quarantine timer");


            await context.Send<INotifyPolice>(msg =>
            {
                msg.PatientId = Data.PatientId;
            });

            MarkAsComplete();

        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<IsolationDataPolicy> mapper)
        {
            mapper.ConfigureMapping<ISentEmail>(message => message.PatientId)
                .ToSaga(sagaData => sagaData.PatientId);

            mapper.ConfigureMapping<INotifiedQuarantine>(message => message.PatientId)
                .ToSaga(sagaData => sagaData.PatientId);
        }

        private async Task NotifyPolice(IMessageHandlerContext context)
        {
            if (Data.IsNotifiedQuarantine && Data.IsSentEmail)
            {
                await context.Send<INotifyPolice>(message =>
                {
                    message.PatientId = Data.PatientId;
                }).ConfigureAwait(false);

                await RequestTimeout(context, TimeSpan.FromSeconds(60), new QuarantineOver { PatientId = Data.PatientId });

            }
        }
    }
}
