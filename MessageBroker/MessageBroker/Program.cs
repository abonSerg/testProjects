using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace MessageBroker
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMqService rabbitMqService = new RabbitMqService();
            IConnection connection = rabbitMqService.GetRabbitMqConnection();
            IModel model = connection.CreateModel();

            SetupInitialTopicQueue(model);
            SendDurableMessageToDurableQueue(model);
        }

        private static void SendDurableMessageToDurableQueue(IModel model)
        {
            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = true;

            byte[] payload = Encoding.UTF8.GetBytes("This is a persistent message from Visual Studio");

            PublicationAddress address = new PublicationAddress(ExchangeType.Topic, "DurableExchange", "durable");

            model.BasicPublish(address, basicProperties, payload);
        }

        private static void SetupInitialTopicQueue(IModel model)
        {
            model.QueueDeclare("DurableQueue", true, false, false, null);
            model.ExchangeDeclare("DurableExchange", ExchangeType.Topic, true);
            model.QueueBind("DurableQueue", "DurableExchange", "durable");
        }
    }
}
