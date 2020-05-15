using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.SMS.SMSIN.Commands;
using Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace WebSmsin.Controllers
{
    public class IndosatController : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<IndosatController>();
        private IOptions<RabbitMQAuth> _RabbitMQAppSetting;

        public IndosatController(IOptions<RabbitMQAuth> RabbitMQAppSetting)
        {
            _RabbitMQAppSetting = RabbitMQAppSetting;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<string> Get([FromQuery] IndosatInRequest request)
        {
            try
            {
                _logger.Information("Request SMSIN: Mottxid ={Motxid}, Mo_Message ={Mo_Message}, Msisdn= {Msisdn}, Operator= {OperatorId}, ShortCod = {Sc}",
                    request.transid, request.sms, request.msisdn, "51021", request.sc);
                //Send TO SMSINQ
                await Mediator.Send(new SendSmsinQueueCommand
                {
                    Motxid = request.transid,
                    Mo_Message = request.sms,
                    Msisdn = request.msisdn,
                    OperatorId = 51021,
                    Shortcode = Convert.ToInt32(request.sc),
                    QueueAuth = _RabbitMQAppSetting.Value
                });

                var response = IndosatInResponse.GetResponse(request.transid);
                _logger.Information("Response string: {response}", response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("ERROR Exeption: " + ex.Message);
                return "BAD REQUEST!";
            }
        }
    }
}