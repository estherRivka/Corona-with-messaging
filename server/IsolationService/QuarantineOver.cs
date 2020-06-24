using NServiceBus;

namespace IsolationService
{
    public  class QuarantineOver: IEvent
    {
        public int PatientId { get; set; }
    }
}