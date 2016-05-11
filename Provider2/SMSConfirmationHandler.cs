using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Provider2
{
    class SMSConfirmationHandler : IHandleMessages<SendSmsConfirmationUsingProvider2>
    {
        public async Task Handle(SendSmsConfirmationUsingProvider2 message, IMessageHandlerContext context)
        {
            Console.WriteLine("Handling SendSmsConfirmationUsingProvider1 with ConfirmationID {0}", message.ConfirmationID);
        }
    }
}