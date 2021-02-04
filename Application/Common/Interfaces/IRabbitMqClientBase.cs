using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IRabbitMqClientBase
    {
        public void ConnectToRabbitMq(string Queue, RabbitMQAuth mQAuth);
        public void Dispose();
    }
}
