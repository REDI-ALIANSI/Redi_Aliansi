using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using Application.Common.Interfaces;
using Common;

namespace Infrastructure
{
    public class MsgQ : IMsgQ
    {
        public async Task<string> ConsumerQueue(string Queue, RabbitMQAuth mQAuth)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = mQAuth.HostName,
                    UserName = mQAuth.UserName,
                    Password = mQAuth.Password,
                    VirtualHost = mQAuth.VirtualHost
                };
                factory.DispatchConsumersAsync = true;
                string message = String.Empty;
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: Queue,
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    BasicGetResult result = channel.BasicGet(Queue, true);
                    if (result == null)
                    {
                        return await Task.FromResult("ERROR : NO MESSAGE FOUND");
                    }
                    else
                    {
                        var body = result.Body.ToArray();
                        message = Encoding.UTF8.GetString(body);
                        return await Task.FromResult(message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetQueueCount(string Queue, RabbitMQAuth mQAuth)
        {
            var factory = new ConnectionFactory
            {
                HostName = mQAuth.HostName,
                UserName = mQAuth.UserName,
                Password = mQAuth.Password,
                VirtualHost = mQAuth.VirtualHost
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                int queueCount = Convert.ToInt32(channel.MessageCount(Queue));
                return await Task.FromResult(queueCount);
            }
        }

        public Task ProducerQueue(object msgObj, string Queue, RabbitMQAuth mQAuth)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = mQAuth.HostName,
                    UserName = mQAuth.UserName,
                    Password = mQAuth.Password,
                    VirtualHost = mQAuth.VirtualHost
                };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: Queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    //Serialize object to JSON
                    string JsonObject = JsonSerializer.Serialize(msgObj);
                    var bodymsg = Encoding.UTF8.GetBytes(JsonObject);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                        routingKey: Queue,
                        basicProperties: null,
                        body: bodymsg);
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
