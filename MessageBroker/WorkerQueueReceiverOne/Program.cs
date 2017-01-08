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
            IConnection connection = messagingService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();
            messagingService.ReceiveWorkerQueueMessages(model);
        }
    }
}
