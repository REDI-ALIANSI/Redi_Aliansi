using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.SMS.SMSIN.Commands;
using Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace WebSmsin.Controllers
{
    public class TselController : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<TselController>();
        private IOptions<RabbitMQAuth> _RabbitMQAppSetting;

        public TselController(IOptions<RabbitMQAuth> RabbitMQAppSetting)
        {
            _RabbitMQAppSetting = RabbitMQAppSetting;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromQuery] TselInRequest request)
        {
            try
            {
                _logger.Information("Request SMSIN: Mottxid ={Motxid}, Mo_Message ={Mo_Message}, Msisdn= {Msisdn}, Operator= {OperatorId}, ShortCod = {Sc}",
                    request.trx_id, request.sms, request.msisdn, "51010", request.adn);
                //Send TO SMSINQ
                await Mediator.Send(new SendSmsinQueueCommand
                {
                    Motxid = request.trx_id,
                    Mo_Message = request.sms,
                    Msisdn = request.msisdn,
                    OperatorId = 51010,
                    Shortcode = Convert.ToInt32(request.adn),
                    QueueAuth = _RabbitMQAppSetting.Value
                });

                var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _logger.Information("Request status: OK");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("ERROR Exeption: " + ex.Message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}