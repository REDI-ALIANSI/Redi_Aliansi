using Domain.Entities.SMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SERVICE.ViewModel
{
    public class CustomServiceSmsinRequest
    {
        public SmsinD smsin { get; set; }
        public List<Message> messages { get; set; }
    }
}
