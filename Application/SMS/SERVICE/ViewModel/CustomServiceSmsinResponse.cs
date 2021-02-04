using Application.Common.Model;
using Domain.Entities.SMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.SMS.SERVICE.ViewModel
{
    public class CustomServiceSmsinResponse
    {
        public List<int> MessageIds { get; set; }
        public string result { get; set; }
    }
}
