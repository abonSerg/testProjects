using System;
using RabbitMqService;
using RabbitMQ.Client;

namespace OneWayMessageSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var messagingService = new AmqpMessagingService();
            var connection = messagingService.GetRabbitMqConnection();
            var model = connection.CreateModel();

            //messagingService.SetUpQueueForOneWayMessageDemo(model);

            RunOneWayMessageDemo(model, messagingService);

            Console.ReadKey();
        }

        private static void RunOneWayMessageDemo(IModel model, AmqpMessagingService messagingService)
        {
            Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");
            while (true)
            {
                var message = Console.ReadLine();
                if (message.ToLower() == "q") break;

                messagingService.SendOneWayMessage(message, model);
            }
        }
    }
}
