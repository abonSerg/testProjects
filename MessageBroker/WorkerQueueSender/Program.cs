using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqService;
using RabbitMQ.Client;

namespace WorkerQueueSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var messagingService = new AmqpMessagingService();
            var connection = messagingService.GetRabbitMqConnection();
            var model = connection.CreateModel();

            messagingService.SetUpQueueForWorkerQueueDemo(model);

            RunWorkerQueueMessageDemo(model, messagingService);
        }

        private static void RunWorkerQueueMessageDemo(IModel model, AmqpMessagingService messagingService)
        {
            Console.WriteLine("Enter your message and press Enter. Quit with 'q'.");
            while (true)
            {
                var message = Console.ReadLine();
                if (message.ToLower() == "q") break;

                messagingService.SendMessageToWorkerQueue(message, model);
            }
        }
    }
}
