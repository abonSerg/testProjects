using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqService;
using RabbitMQ.Client;

namespace WorkerQueueReceiverOne
{
    class Program
    {
        static void Main(string[] args)
        {
            var messagingService = new AmqpMessagingService();
            var connection = messagingService.GetRabbitMqConnection();
            var model = connection.CreateModel();

            messagingService.ReceiveWorkerQueueMessages(model);
        }
    }
}
