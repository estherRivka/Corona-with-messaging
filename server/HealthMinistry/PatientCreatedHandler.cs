
using CoronaApp.Api.Exceptions;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Threading.Tasks;

namespace HealthMinistryService
{
    public class PatientCreatedHandler : IHandleMessages<IPatientCreated>
    {
        static Random random = new Random();

        static ILog log = LogManager.GetLogger<IPatientCreated>();
        public Task Handle(IPatientCreated message, IMessageHandlerContext context)
        {
            log.Info($"Received PatientCreated, PatientId = {message.PatientId}");


            //throw new PatientNotExistExcption();
            //throw new Exception();

            return Task.CompletedTask;


        }
    }
}
