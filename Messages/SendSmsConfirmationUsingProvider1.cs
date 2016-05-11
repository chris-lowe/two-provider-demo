using System;
using NServiceBus;

namespace Messages
{
    public class SendSmsConfirmationUsingProvider1 : ICommand
    {
        public Guid ConfirmationID { get; set; }
        public string ConfirmationCode { get; set; }
        public string MobileNumber { get; set; }
    }
}
