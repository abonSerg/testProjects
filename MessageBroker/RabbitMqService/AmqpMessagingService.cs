using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace RabbitMqService
{
    public class AmqpMessagingService
    {
        private string _hostName = "localhost";
        private string _userName = "guest";
        private string _password = "guest";
        private string _exchangeName = "";
        private string _oneWayMessageQueueName = "OneWayMessageQueue";
        private string _workerQueueDemoQueueName = "WorkerQueueDemoQueue";
        private string _publishSubscribeExchangeName = "PublishSubscribeExchange";
        private string _publishSubscribeQueueOne = "PublishSubscribeQueueOne";
        private string _publishSubscribeQueueTwo = "PublishSubscribeQueueTwo";
        private bool _durable = true;

        public IConnection GetRabbitMqConnection()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            return connectionFactory.CreateConnection();
        }

        public void SetUpQueueForOneWayMessageDemo(IModel model)
        {
            model.QueueDeclare(_oneWayMessageQueueName, _durable, false, false, null);
        }

        public void SetUpQueueForWorkerQueueDemo(IModel model)
        {
            model.QueueDeclare(_workerQueueDemoQueueName, _durable, false, false, null);
        }


        public void SendOneWayMessage(string message, IModel model)
        {
            var basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = _durable;

            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(_exchangeName, _oneWayMessageQueueName, basicProperties, messageBytes);
        }

        public void ReceiveOneWayMessages(IModel model)
        {
            ReceiveQueueMessages(model, _workerQueueDemoQueueName);
        }



        public void ReceiveWorkerQueueMessages(IModel model)
        {
            ReceiveQueueMessages(model, _workerQueueDemoQueueName);
        }

        private void ReceiveQueueMessages(IModel model, string queueName)
        {
            model.BasicQos(0, 1, false);
            var consumer = new QueueingBasicConsumer(model);

            model.BasicConsume(queueName, false, consumer);
            while (true)
            {
                var deliveryArguments = consumer.Queue.Dequeue();
                var message = Encoding.UTF8.GetString(deliveryArguments.Body);

                Console.WriteLine("Message received: {0}", message);

                model.BasicAck(deliveryArguments.DeliveryTag, false);
            }
        }

        public void SendMessageToWorkerQueue(string message, IModel model)
        {
            var basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = _durable;

            var messageBytes = Encoding.UTF8.GetBytes(message);

            model.BasicPublish(_exchangeName, _workerQueueDemoQueueName, basicProperties, messageBytes);
        }

        public void SetUpExchangeAndQueuesForDemo(IModel model)
        {
            model.ExchangeDeclare(_publishSubscribeExchangeName, ExchangeType.Fanout, true);
            model.QueueDeclare(_publishSubscribeQueueOne, true, false, false, null);
            model.QueueDeclare(_publishSubscribeQueueTwo, true, false, false, null);
            model.QueueBind(_publishSubscribeQueueOne, _publishSubscribeExchangeName, "");
            model.QueueBind(_publishSubscribeQueueTwo, _publishSubscribeExchangeName, "");
        }

        public void SendMessageToPublishSubscribeQueue(string message, IModel model)
        {
            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = _durable;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(_publishSubscribeExchangeName, "", basicProperties, messageBytes);
        }

        public void ReceivePublishSubscribeMessageReceiverOne(IModel model)
        {
            model.BasicQos(0, 1, false);
            Subscription subscription = new Subscription(model, _publishSubscribeQueueOne, false);
            while (true)
            {
                BasicDeliverEventArgs deliveryArguments = subscription.Next();
                String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                Console.WriteLine("Message from queue: {0}", message);
                subscription.Ack(deliveryArguments);
            }
        }

        public void ReceivePublishSubscribeMessageReceiverTwo(IModel model)
        {
            model.BasicQos(0, 1, false);
            Subscription subscription = new Subscription(model, _publishSubscribeQueueTwo, false);
            while (true)
            {
                BasicDeliverEventArgs deliveryArguments = subscription.Next();
                String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                Console.WriteLine("Message from queue: {0}", message);
                subscription.Ack(deliveryArguments);
            }
        }
    }

  
}
