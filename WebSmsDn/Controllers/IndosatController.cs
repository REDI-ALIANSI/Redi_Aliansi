using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.SMS.SMSDN.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebSmsDn.Controllers
{
    public class IndosatController : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<IndosatController>();

        [HttpGet]
        [AllowAnonymous]
        public async Task<string> Get([FromQuery] IndosatDnRequest request)
        {
            try
            {
                _logger.Information("Request DN: time ={time}, serviceid ={serviceid}, dest= {dest}, tid= {tid},status= {status}, Operator= {Operator}",
                    request.time, request.serviceid, request.dest, request.tid, request.status, "51021");

                string Dnstatus = String.Empty;
                if (request.status.Equals("2"))
                {
                    Dnstatus = "Delivered";
                }
                else Dnstatus = "Failed";
                //Send TO SMSINQ
                await Mediator.Send(new InsertDnRequest
                {
                    DnErrorcode = request.status,
                    DnMtid = request.tid,
                    Status = Dnstatus
                });

                var response = IndosatDnResponse.GetResponse(request.tid,request.status);
                _logger.Information("Response string: {response}", response);
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("ERROR Exeption: " + ex.ToString());
                return "BAD REQUEST!";
            }
        }
    }
}