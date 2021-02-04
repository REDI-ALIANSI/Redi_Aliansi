using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Model;
using Application.SMS.SERVICE.ViewModel;
using Application.SMS.SMSOUT.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using MediatR;
using System.Collections.Generic;
using Application.SMS.MESSAGE.Queries;

namespace WebApiSMS.Controllers
{
    public class Hedi2Controller : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<Hedi2Controller>();

        [HttpPost]
        [AllowAnonymous]
        public async Task<CustomServiceSmsinResponse> Smsin([FromBody] CustomServiceSmsinRequest rsmsin)
        {
            var response = new CustomServiceSmsinResponse();

            try
            {
                _logger.Information("smsin: MO: " + rsmsin.smsin.Mo_Message
                    + " Msisdn: " + rsmsin.smsin.Msisdn
                    + " Operator: " + rsmsin.smsin.OperatorId
                    + " Serviceid: " + rsmsin.smsin.ServiceId);
                int i = 1;
                foreach (var message in rsmsin.messages)
                {
                    _logger.Information("Message no " + i
                        + " Type: " + message.MessageType
                        + " Messagetxt: " + message.MessageTxt);
                    i++;
                }
                
                var LastMessageOut = await Mediator.Send(new GetLastSmsoutD
                {
                    Msisdn = rsmsin.smsin.Msisdn,
                    OperatorId = rsmsin.smsin.OperatorId
                });

                var MessageListId = new List<int>();

                //Set Message ID For Confirmation Weekly
                if (!(LastMessageOut is null) && LastMessageOut.MessageId.Equals(37)) //this is prod
                //if (!(LastMessageOut is null) && LastMessageOut.MessageId.Equals(20)) //this is dev
                {
                    var respMessage = rsmsin.messages.Where(m => m.Order.Equals(2)).FirstOrDefault();
                    MessageListId.Add(respMessage.MessageId);
                    
                    var MessagePushConfirm = await Mediator.Send(new GetMessagesByTypeAndOrder
                    { 
                        MessageType = "FREEPUSH", 
                        Serviceid = 5, 
                        OperatorId = rsmsin.smsin.OperatorId,
                        Order = 1 });
                    MessageListId.Add(MessagePushConfirm.MessageId);

                    var MessagePushDownloadConfirm = await Mediator.Send(new GetMessagesByTypeAndOrder
                    {
                        MessageType = "FREEPUSH",
                        Serviceid = 5,
                        OperatorId = rsmsin.smsin.OperatorId,
                        Order = 2
                    });
                    MessageListId.Add(MessagePushDownloadConfirm.MessageId);

                    response.MessageIds = MessageListId;
                }
                else
                {
                    var respMessage = rsmsin.messages.Where(m => m.Order.Equals(2)).FirstOrDefault();
                    response.MessageIds.Add(respMessage.MessageId);
                }

                response.result = "OK";
                _logger.Information("Custom Smsin response :");
                foreach (var message in response.MessageIds)
                    _logger.Information("MessageId:{0}", message.ToString());
                _logger.Information("Response Status : {0}", response.result);
                return response;
            }
            catch (Exception ex)
            {
                //string[] ErrorMessage = new string[] { ex.Message };
                response.result = "ERROR: Check error on WebApiSMS log! ";
                _logger.Error("Error : " + ex.Message);
                return response;
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<CustomServiceDnWatchResponse> DnWatch(CustomServiceDnwatchRequest rDnwatch)
        //{
        //    var response = new CustomServiceDnWatchResponse();

        //    try
        //    {

        //        //Do Custom DNWatch logic here 
        //        if (rDnwatch.smsdn.Status.ToLower().Equals("delivrd"))
        //        {
        //            StringBuilder strUrl = new StringBuilder();
        //            var Subs = await Mediator.Send(new GetSubscription
        //            {
        //                Msisdn = rDnwatch.smsdn.SmsoutD.Msisdn,
        //                OperatorId = rDnwatch.smsdn.SmsoutD.OperatorId,
        //                ServiceId = rDnwatch.smsdn.SmsoutD.ServiceId
        //            });
        //            string[] arrKeyword = Subs.Reg_Keyword.Split(' ');
        //            strUrl.Append("http://offer.mobhauz.com/trackapps/campaign/postback/?type=mt");
        //            strUrl.Append("&transaction_id=" + arrKeyword[3]);
        //            _logger.Information("HIT URL POSTBACK First Push RN: " + strUrl);
        //            string resp = await Mediator.Send(new CallBackRequest { rUrl = strUrl.ToString() });
        //            _logger.Information("RESP URL POSTBACK First Push RN: " + resp);
        //        }
        //        else
        //        {
        //            _logger.Information("DN Failed status: " + rDnwatch.smsdn.Status.ToLower());
        //        }
        //        response.result = Result.Success();
        //        _logger.Information("DnWatch success!");
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        string[] ErrorMessage = new string[] { ex.Message };
        //        response.result = Result.Failure(ErrorMessage);
        //        _logger.Error("Error : " + ex.Message);
        //        return response;
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<CustomServiceRenewalResponse> Renewal(CustomServiceRenewalRequest rRenewal)
        //{
        //    var response = new CustomServiceRenewalResponse();

        //    try
        //    {

        //        //Do Custom Renewal logic here 

        //        response.result = Result.Success();
        //        _logger.Information("Renewal Custom success!");
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        string[] ErrorMessage = new string[] { ex.Message };
        //        response.result = Result.Failure(ErrorMessage);
        //        return response;
        //    }
        //}
    }
}
