using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Model;
using Application.SMS.CALLBACK.Commands;
using Application.SMS.CONTENT.Command;
using Application.SMS.SERVICE.ViewModel;
using Application.SMS.SUBSCRIPTION.Queries;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace WebApiSMS.Controllers
{
    public class TestController : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<TestController>();
        private readonly IOptions<Bitly_Api> _Bitly_Api;

        public TestController(IOptions<Bitly_Api> Bitly_Api)
        {
            _Bitly_Api = Bitly_Api;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<CustomServiceSmsinResponse> Smsin([FromBody]CustomServiceSmsinRequest rsmsin)
        {
            var response = new CustomServiceSmsinResponse();

            try
            {
                //Split the MO with SPACE as delimiter
                //string[] arrKeyword = rsmsin.smsin.Mo_Message.Split(' ');

                //Callback
                //if (rsmsin.smsin.Mo_Message.Contains(" RN"))
                //{
                //    StringBuilder strUrl = new StringBuilder();
                //    strUrl.Append("http://pb.gocloudtracker.com/receive/?");
                //    strUrl.Append("transaction_id=" + arrKeyword[3]);
                //    _logger.Information("HIT URL POSTBACK RNB/EKO: " + strUrl);
                //    string resp = await Mediator.Send(new CallBackRequest { rUrl = strUrl.ToString() });
                //    _logger.Information("RESP URL POSTBACK RNB/EKO: " + resp);
                //}

                var Message = rsmsin.messages.FirstOrDefault();
                //Api to send to SMS
                string UrlContent = String.Empty;
                string Api_token = _Bitly_Api.Value.Api_Token;

                var ShortUrl = await Mediator.Send(new ShortenUrl { LongUrl = UrlContent, Token = Api_token });
                _logger.Information("Short URL : " + ShortUrl);

                Message.MessageTxt = Message.MessageTxt.Replace("%P1%", ShortUrl);

                response.result = "OK";
                _logger.Information("Message success!");
                return response;
            }
            catch(Exception ex)
            {
                //string[] ErrorMessage = new string[] { ex.Message };
                response.result = "ERROR: Check error on WebApiSMS log! ";
                _logger.Error("Error : " + ex.Message);
                return response;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<CustomServiceDnWatchResponse> DnWatch(CustomServiceDnwatchRequest rDnwatch)
        {
            var response = new CustomServiceDnWatchResponse();

            try
            {

                //Do Custom DNWatch logic here 
                if (rDnwatch.smsdn.Status.ToLower().Equals("delivrd"))
                {
                    StringBuilder strUrl = new StringBuilder();
                    var Subs = await Mediator.Send(new GetSubscription 
                                    { 
                                        Msisdn = rDnwatch.smsdn.SmsoutD.Msisdn, 
                                        OperatorId = rDnwatch.smsdn.SmsoutD.OperatorId, 
                                        ServiceId = rDnwatch.smsdn.SmsoutD.ServiceId 
                                    });
                    string[] arrKeyword = Subs.Reg_Keyword.Split(' ');
                    strUrl.Append("http://offer.mobhauz.com/trackapps/campaign/postback/?type=mt");
                    strUrl.Append("&transaction_id=" + arrKeyword[3]);
                    _logger.Information("HIT URL POSTBACK First Push RN: " + strUrl);
                    string resp = await Mediator.Send(new CallBackRequest { rUrl = strUrl.ToString() });
                    _logger.Information("RESP URL POSTBACK First Push RN: " + resp);
                }
                else
                {
                    _logger.Information("DN Failed status: " + rDnwatch.smsdn.Status.ToLower());
                }
                response.result = Result.Success();
                _logger.Information("DnWatch success!");
                return response;
            }
            catch (Exception ex)
            {
                string[] ErrorMessage = new string[] { ex.Message };
                response.result = Result.Failure(ErrorMessage);
                _logger.Error("Error : " + ex.Message);
                return response;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<CustomServiceRenewalResponse> Renewal(CustomServiceRenewalRequest rRenewal)
        {
            var response = new CustomServiceRenewalResponse();

            try
            {

                //Do Custom Renewal logic here 

                response.result = Result.Success();
                _logger.Information("Renewal Custom success!");
                return response;
            }
            catch (Exception ex)
            {
                string[] ErrorMessage = new string[] { ex.Message };
                response.result = Result.Failure(ErrorMessage);
                return response;
            }
        }
    }
}
