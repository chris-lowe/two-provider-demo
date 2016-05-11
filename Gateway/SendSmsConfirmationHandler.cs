using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Gateway
{
    class SendSmsConfirmationHandler:IHandleMessages<SendSmsConfirmation>
    {
        public async Task Handle(SendSmsConfirmation message, IMessageHandlerContext context)
        {
            switch (Program.SMSProvider)
            {
                case 1:
                    await context.Send("POC.Provider1", new SendSmsConfirmationUsingProvider1 {ConfirmationID = message.ConfirmationID, MobileNumber = message.MobileNumber, ConfirmationCode = message.ConfirmationCode });
                    Console.WriteLine("Message SendSmsConfirmationUsingProvider1 sent with confirmationID {0}", message.ConfirmationID);
                    break;
                case 2:
                    await context.Send("POC.Provider2", new SendSmsConfirmationUsingProvider2 { ConfirmationID = message.ConfirmationID, MobileNumber = message.MobileNumber, ConfirmationCode = message.ConfirmationCode });
                    Console.WriteLine("Message SendSmsConfirmationUsingProvider2 sent with confirmationID {0}", message.ConfirmationID);
                    break;
                default:
                    return;
            }
        }
    }
}