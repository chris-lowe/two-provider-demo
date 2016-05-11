using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus.Persistence;

namespace Client
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "POC BusinessLayer";

            #region SenderConfiguration

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("POC.BusinessLayer");
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
                Console.WriteLine("Press any key to send a message");
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

                    await endpoint.Send("POC.Gateway", new SendSmsConfirmation
                    {
                        ConfirmationID = confirmationID,
                        MobileNumber = "07711606060",
                        ConfirmationCode = "2345"
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
