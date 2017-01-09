using RabbitMqService;
using RabbitMQ.Client;

namespace WorkerQueueReceiverTwo
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
