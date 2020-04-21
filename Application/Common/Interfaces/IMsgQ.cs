using Domain.Entities.SMS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IMsgQ
    {
        Task ProducerQueue(object msgObj, string Queue);
        Task<string> ConsumerQueue(string Queue);
        Task<int> GetQueueCount(string Queue);
    }
}
