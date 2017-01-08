using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
        private bool _durable = true;

        public IConnection GetRabbitMqConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.HostName = _hostName;
            connectionFactory.UserName = _userName;
            connectionFactory.Password = _password;

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
            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = _durable;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(_exchangeName, _oneWayMessageQueueName, basicProperties, messageBytes);
        }

        public void ReceiveOneWayMessages(IModel model)
        {
            model.BasicQos(0, 1, false); //basic quality of service
            var consumer = new QueueingBasicConsumer(model);
            //   var consumer = new EventingBasicConsumer(model);
            model.BasicConsume(_oneWayMessageQueueName, false, consumer);
            while (true)
            {
                BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue();
                string message = Encoding.UTF8.GetString(deliveryArguments.Body);
                Console.WriteLine("Message received: {0}", message);
                model.BasicAck(deliveryArguments.DeliveryTag, false);
            }
        }
        public void SendMessageToWorkerQueue(string message, IModel model)
        {
            IBasicProperties basicProperties = model.CreateBasicProperties();
            basicProperties.SetPersistent(_durable);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            model.BasicPublish(_exchangeName, _workerQueueDemoQueueName, basicProperties, messageBytes);
        }

        public void ReceiveWorkerQueueMessages(IModel model)
        {
            model.BasicQos(0, 1, false); //basic quality of service
            QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
            model.BasicConsume(_workerQueueDemoQueueName, false, consumer);
            while (true)
            {
                BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
                String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                Console.WriteLine("Message received: {0}", message);
                model.BasicAck(deliveryArguments.DeliveryTag, false);
            }
        }
    }

  
}
