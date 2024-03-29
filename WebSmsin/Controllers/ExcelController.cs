﻿using System;
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
    public class ExcelController : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<ExcelController>();
        private readonly IOptions<RabbitMQAuth> _RabbitMQAppSetting;

        public ExcelController(IOptions<RabbitMQAuth> RabbitMQAppSetting)
        {
            _RabbitMQAppSetting = RabbitMQAppSetting;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Get([FromQuery] ExcelInRequest request)
        {
            HttpResponseMessage result;
            //Get the query string
            try
            {
                int GetShortCode = Convert.ToInt32(this.HttpContext.Request.Query["X-Dest-Addr"]);
                string GetMsisdn = String.Empty;
                if (!String.IsNullOrEmpty(this.HttpContext.Request.Query["X-Source-Addr"]))
                {
                    GetMsisdn = this.HttpContext.Request.Query["X-Source-Addr"];
                    if (GetMsisdn.ToLower().Substring(0, 4).Equals("tel:"))
                    {
                        int count = GetMsisdn.Length - 4;
                        GetMsisdn = GetMsisdn.Substring(4, count);
                    }
                }

                string GetTrxId = String.Empty;
                if (!String.IsNullOrEmpty(this.HttpContext.Request.Query["X-Pull-Trx-Id"]))
                {
                    GetTrxId = this.HttpContext.Request.Query["X-Pull-Trx-Id"];
                }
                else
                {
                    GetTrxId = request._TID;
                }

                _logger.Information("Request SMSIN: Mottxid ={Motxid}, Mo_Message ={Mo_Message}, Msisdn= {Msisdn}, Operator= {OperatorId}, ShortCod = {Sc}",
                    GetTrxId, request._SC, GetMsisdn, "51011", GetShortCode);

                //Send TO SMSINQ
                await Mediator.Send(new SendSmsinQueueCommand
                {
                    Motxid = GetTrxId,
                    Mo_Message = request._SC,
                    Msisdn = GetMsisdn,
                    OperatorId = 51011,
                    Shortcode = GetShortCode,
                    QueueAuth = _RabbitMQAppSetting.Value
                });
                result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _logger.Information("Request status: OK");
            }
            catch (Exception ex)
            {
                _logger.Error("ERROR Exeption: " + ex.Message);
                result = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            return result;
        }
    }
}