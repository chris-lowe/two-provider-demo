﻿using System;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus;
using NServiceBus.Persistence;

namespace BusinessLayer
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
                Console.WriteLine("Business Layer");
                Console.WriteLine("Waiting for SendSmsConfirmation confirmation command");
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