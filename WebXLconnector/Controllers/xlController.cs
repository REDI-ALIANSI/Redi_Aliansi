using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.SMS.SMSDN.Commands;
using Application.SMS.SMSIN.Commands;
using Application.SMS.SMSOUT.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebXLconnector.Controllers
{
    [Route("api/[controller]")]
    public class xlController : Controller
    {
        private readonly IOptions<XlAPI> _XlAPI;
        private readonly IHttpRequest _httpRequest;

        public xlController(IOptions<XlAPI> XlAPI, IHttpRequest httpRequest)
        {
            _XlAPI = XlAPI;
            _httpRequest = httpRequest;
        }

        [HttpGet("mo")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Mo([FromQuery] ExcelInRequest request)
        {
            HttpResponseMessage result;
            var QueryString = this.Request.QueryString.ToUriComponent();
            
            StringBuilder XLsmsin = new StringBuilder();
            XLsmsin.Append(_XlAPI.Value.SmsinEndPoint);
            XLsmsin.Append(QueryString);

            try
            {
                result = await _httpRequest.GetHttpResp(XLsmsin.ToString());
            }
            catch (Exception)
            {
                result = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            return result;
        }

        [HttpGet("mt")]
        [AllowAnonymous]
        public async Task<string> Mt([FromQuery] XlSmsoutConReq req)
        {
            var QueryString = this.Request.QueryString.ToUriComponent();

            StringBuilder XLsmsin = new StringBuilder();
            XLsmsin.Append(_XlAPI.Value.SmsoutEndPoint);
            XLsmsin.Append(QueryString);

            string result;
            try
            {
                result = await _httpRequest.GetRequest(XLsmsin.ToString());
            }
            catch (Exception ex)
            {
                result = "REQUEST ERROR : " + ex.ToString();
            }

            return result;
        }

        [HttpGet("dn")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Dn([FromQuery] ExcelDrRequest request)
        {
            HttpResponseMessage result;
            var QueryString = this.Request.QueryString.ToUriComponent();

            StringBuilder XLsmsin = new StringBuilder();
            XLsmsin.Append(_XlAPI.Value.SmsdnEndPoint);
            XLsmsin.Append(QueryString);

            try
            {
                result = await _httpRequest.GetHttpResp(XLsmsin.ToString());
            }
            catch (Exception)
            {
                result = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

            return result;
        }
    }
}