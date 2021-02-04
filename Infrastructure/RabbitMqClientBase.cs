using Application.Common.Interfaces;
using Common;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class RabbitMqClientBase : IRabbitMqClientBase
    {
        protected IModel Channel { get; private set; }
        private IConnection _connection;
        private ConnectionFactory _connectionFactory;

        public RabbitMqClientBase(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void ConnectToRabbitMq(string Queue, RabbitMQAuth mQAuth)
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connectionFactory = new ConnectionFactory
                {
                    HostName = mQAuth.HostName,
                    UserName = mQAuth.UserName,
                    Password = mQAuth.Password,
                    VirtualHost = mQAuth.VirtualHost
                };
                _connection = _connectionFactory.CreateConnection();
            }

            if (Channel == null || Channel.IsOpen == false)
            {
                Channel = _connection.CreateModel();
                Channel.QueueDeclarePassive(Queue);
                //Channel.QueueBind(queue: Queue, exchange: LoggerExchange, routingKey: LoggerQueueAndExchangeRoutingKey);
            }
        }

        public void Dispose()
        {
            try
            {
                Channel?.Close();
                Channel?.Dispose();
                Channel = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
