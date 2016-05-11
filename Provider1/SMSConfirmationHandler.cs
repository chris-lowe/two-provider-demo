using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Provider1
{
    class SMSConfirmationHandler :IHandleMessages<SendSmsConfirmationUsingProvider1>
    {
        public async Task Handle(SendSmsConfirmationUsingProvider1 message, IMessageHandlerContext context)
        {
            Console.WriteLine("Handling SendSmsConfirmationUsingProvider1 with ConfirmationID {0}", message.ConfirmationID);
        }
    }
}