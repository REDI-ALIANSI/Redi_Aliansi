using Common;
using Domain.Entities.SMS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMsgQ
    {
        Task ProducerQueue(object msgObj, string Queue, RabbitMQAuth mQAuth);
        Task<string> ConsumerQueue(string Queue, RabbitMQAuth mQAuth);
        Task<int> GetQueueCount(string Queue, RabbitMQAuth mQAuth);
    }
}
