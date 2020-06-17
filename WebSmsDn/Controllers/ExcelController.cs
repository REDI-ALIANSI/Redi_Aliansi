using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.SMS.SMSDN.Commands;
using Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace WebSmsDn.Controllers
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
        public async Task<HttpResponseMessage> Get([FromQuery] ExcelDrRequest request)
        {
            try
            {
                _logger.Information("Request DN: _tid ={_tid}, status_id ={status_id}, dtdone= {dtdone}, errorcode= {errorcode},errordescription= {errordescription}, sid={sid}, Operator= {Operator}",
                    request._tid, request.status_id, request.dtdone, request.errorcode, request.errordescription,request.sid, "51021");

                string Dnstatus = String.Empty;
                string ErrorCode = String.Empty;

                if (request.errorcode != null)
                {
                    ErrorCode = request.errorcode.Substring(request.errorcode.Count<char>() - 5);
                }
                else ErrorCode = "1";

                if (request.status_id.Equals("102"))
                {
                    Dnstatus = "Delivered";
                }
                else Dnstatus = "Failed";
                //Send TO SMSINQ
                await Mediator.Send(new InsertDnRequest
                {
                    DnErrorcode = ErrorCode,
                    DnMtid = request._tid,
                    Status = Dnstatus,
                    QueueAuth = _RabbitMQAppSetting.Value
                });

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                _logger.Information("Response string: {response}", "OK");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("ERROR Exeption: " + ex.ToString());
                throw ex;
            }
        }
    }
}