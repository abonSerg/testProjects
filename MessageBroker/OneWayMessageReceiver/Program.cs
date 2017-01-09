using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqService;
using RabbitMQ.Client;

namespace OneWayMessageReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var messagingService = new AmqpMessagingService();
            var connection = messagingService.GetRabbitMqConnection();

            var model = connection.CreateModel();
            messagingService.ReceiveOneWayMessages(model);
        }
    }
}
