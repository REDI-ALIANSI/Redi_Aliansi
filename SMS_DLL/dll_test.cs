using Application.Common.Exceptions;
using Domain.Entities.SMS;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMS_DLL
{
    public class dll_test
    {
        public List<Message> Smsin(SmsinD smsinD,List<Message> messages)
        {
            try
            {
                string[] arrKeyword = smsinD.Mo_Message.Split(' ');
                string type = String.Empty;
                string response = "OK";
                if (arrKeyword.Length > 1)
                {
                    if (arrKeyword[0].ToUpper().Equals("REG"))
                    {
                        type = "ON";
                    }
                    else if (arrKeyword[0].ToUpper().Equals("UNREG") || arrKeyword[0].ToUpper().Equals("OFF"))
                    {
                        type = "OFF";
                    }
                    else if (arrKeyword[0].ToUpper().Equals("HELP"))
                    {
                        type = "HELP";
                    }
                    else type = "PULL";
                }
                else
                {
                    if (smsinD.Mo_Message.ToUpper().Equals("HELP"))
                    {
                        type = "HELP";
                    }
                    else type = "PULL";
                }

                if (messages.Count() > 0)
                {
                    foreach (var message in messages)
                    {
                        //do something to each messages here
                        
                    }
                }
                else throw new NotFoundException(nameof(Message), messages);

                //Do needed callback here
                if (type.Equals("REG") && arrKeyword.Length > 1 && arrKeyword[1].Equals("test"))
                {
                    //call callback
                    string Callback = "http://google.com";
                    //response = HttpRequest.GetRequest(Callback);
                    messages.FirstOrDefault().Sparam = response;
                }

                return messages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
