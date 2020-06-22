using NServiceBus;

namespace Messages.Events
{
    //[Table("SubmittedOrder", Schema = "receiver")]
    public interface IPatientCreated 
    { 

         int PatientId { get; set; }

    }
}