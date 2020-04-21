using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.SMS.SMSIN.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSmsin.Controllers
{
    public class TselController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromQuery] TselInRequest request)
        {
            try
            {
                //Send TO SMSINQ
                await Mediator.Send(new SendSmsinQueueCommand
                {
                    Motxid = request.trx_id,
                    Mo_Message = request.sms,
                    Msisdn = request.msisdn,
                    OperatorId = 51010,
                    Shortcode = Convert.ToInt32(request.adn)
                });

                var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}