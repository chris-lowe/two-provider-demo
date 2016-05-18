using System;
using System.Threading;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace BusinessLayer
{
    public class SMSConfirmationSaga : Saga<SMSConfirmationData>,
        IAmStartedByMessages<SendSmsConfirmation>,
        IHandleMessages<SmsConfirmationComplete>
    {

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SMSConfirmationData> mapper)
        {
            mapper.ConfigureMapping<SendSmsConfirmation>(message => message.ConfirmationID)
                .ToSaga(sagaData => sagaData.ConfirmationID);

            mapper.ConfigureMapping<SmsConfirmationComplete>(message => message.ConfirmationID)
                .ToSaga(sagaData => sagaData.ConfirmationID);
        }

        public async Task Handle(SendSmsConfirmation message, IMessageHandlerContext context)
        {
            Data.ConfirmationID = message.ConfirmationID;
            Data.CustomerID = message.CustomerID;
            Data.MobileNumber = message.MobileNumber;
            // some component generates the key.
            message.ConfirmationCode = "2345";
            await context.Send("POC.Gateway", message);
            Console.WriteLine("Saga started for message {0}", message.ConfirmationID);
        }

        public async Task Handle(SmsConfirmationComplete message, IMessageHandlerContext context)
        {
            // Publish an event to say that this has happened
            // context.publish("myEndpoint",  new SMSConfirmed( CustomerID = data.customerID, MobileNumber = Data.MobileNumber
            MarkAsComplete();
        }

    }
}