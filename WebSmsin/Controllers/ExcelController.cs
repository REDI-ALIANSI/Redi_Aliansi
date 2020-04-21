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
    public class ExcelController : BaseController
    {
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

                //Send TO SMSINQ
                await Mediator.Send(new SendSmsinQueueCommand
                {
                    Motxid = GetTrxId,
                    Mo_Message = request._SC,
                    Msisdn = GetMsisdn,
                    OperatorId = 51011,
                    Shortcode = GetShortCode
                });
                result = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            return result;
        }
    }
}