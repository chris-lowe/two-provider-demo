using System;
using NServiceBus;

namespace Messages
{
    public class SmsConfirmationComplete : IMessage
    {
        public Guid ConfirmationID { get; set; }
    }
}
