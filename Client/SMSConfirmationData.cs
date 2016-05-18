using System;
using NServiceBus;

namespace BusinessLayer
{
    public class SMSConfirmationData : ContainSagaData
    {
        public virtual Guid CustomerID{ get; set; }
        public virtual Guid ConfirmationID { get; set; }
        public virtual string MobileNumber { get; set; }

        //public virtual Guid Id { get; set; }
        //public virtual string OriginalMessageId { get; set; }
        //public virtual string Originator { get; set; }


    }
}