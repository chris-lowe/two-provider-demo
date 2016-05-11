using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Persistence;

namespace Provider2
{
    class Program
    {
        private readonly SMSConfirmationHandler _smsConfirmationHandler = new SMSConfirmationHandler();

        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "POC Provider 2";

            #region GatewayConfiguration

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("POC.Provider2");
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
                Console.WriteLine("Sms Provider 2");
                Console.WriteLine("Waiting for SMS confirmation messages");
                Console.WriteLine("Press enter key to exit");

                while (true)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    Console.WriteLine();

                    if (input.Key == ConsoleKey.Enter)
                    {
                        return;
                    }
                }
            }

            finally
            {
                await endpoint.Stop();
            }
        }
    }
}