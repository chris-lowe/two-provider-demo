using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Persistence;

namespace Gateway
{
    class Program
    {
        private readonly SendSmsConfirmationHandler _sendSmsConfirmationHandler = new SendSmsConfirmationHandler();

        public static int SMSProvider { get; set; }

        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "POC Gateway";

            #region GatewayConfiguration

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("POC.Gateway");
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

            SMSProvider = 1;

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

            try
            {
                Console.WriteLine("The Default SMSprovider is Provider1");
                Console.WriteLine("Press space bar to switch provider");
                Console.WriteLine("Press enter key to exit");

                while (true)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    Console.WriteLine();

                    switch (input.Key)
                    {
                        case ConsoleKey.Enter:
                            return;
                        case ConsoleKey.Spacebar:
                            if (SMSProvider == 2)
                            {
                                SMSProvider = 1;
                                Console.WriteLine("Sms Provider has been swictched to provider {0}", 1);
                            }
                            else
                            {
                                SMSProvider = 2;
                                Console.Write("Sms Provider has been swictched to provider {0}", 2);
                            }
                            break;
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
