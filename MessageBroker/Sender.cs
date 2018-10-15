using System;
using System.Collections.Concurrent;
using System.Text;
using RabbitMQ.Client;

namespace MessageBroker
{
    public class Sender : ISender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _exchangeName;

        public string Type { get; }

        public Sender(string type, string host, string userName, string password, string queueName, string exchangeName)
        {
            Type = type;
            _host = host;
            _userName = userName;
            _password = password;
            _queueName = queueName;
            _exchangeName = exchangeName;
        }

        public void SendMessage(string message)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _host,
                UserName = _userName,
                Password = _password
            };

            using (var connection = connectionFactory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {
                    model.ExchangeDeclare(_exchangeName, ExchangeType.Direct, true);
                    model.QueueDeclare(_queueName, true, false, false, new ConcurrentDictionary<string, object>());
                    model.QueueBind(_queueName, _exchangeName, _queueName, null);

                    byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
                    model.BasicPublish(_exchangeName, _queueName, null, messageBodyBytes);
                }
            }
        }
    }
}
