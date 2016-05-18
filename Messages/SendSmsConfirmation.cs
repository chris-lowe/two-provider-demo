using System;
using NServiceBus;

namespace Messages
{
    public class SendSmsConfirmation : ICommand
    {

        public Guid ConfirmationID { get; set; }
        public Guid CustomerID { get; set; }
        public string ConfirmationCode { get; set; }
        public string MobileNumber { get; set; }
    }
}
