using System;
using System.Threading.Tasks;
using Messages;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Persistence;

namespace UI
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "POC UI";

            #region SenderConfiguration

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("POC.UI");
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            Configuration hibernateConfig = new Configuration();
            hibernateConfig.DataBaseIntegration(x =>
            {
                x.ConnectionStringName = "NServiceBus/Persistence";
                x.Dialect<MsSql2012Dialect>();
            });
            hibernateConfig.SetProperty("default_schema", "dbo");

            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
            persistence.UseConfiguration(hibernateConfig);

            #endregion

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

            try
            {
                Console.WriteLine("Press any key to send the SmsConfirmation command");
                Console.WriteLine("Press enter to exit");

                while (true)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    Console.WriteLine();

                    if (input.Key == ConsoleKey.Enter)
                    {
                        return;
                    }

                    var confirmationID = Guid.NewGuid();

                    await endpoint.Send("POC.BusinessLayer", new SendSmsConfirmation
                    {
                        ConfirmationID = confirmationID,
                        CustomerID = Guid.NewGuid(),
                        MobileNumber = "07711606060",
                    });

                    Console.Write("SendSmsConfirmation message sent with confirmationID {0}", confirmationID);
                }
            }
            finally
            {
                await endpoint.Stop();
            }
        }
    }
}
