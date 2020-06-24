using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace IsolationService
{
   public class IsolationDataPolicy : ContainSagaData
    {
        public bool IsNotifiedQuarantine { get; set; }
        public bool IsSentEmail { get; set; }

        public int PatientId { get; set; }

    }
}
