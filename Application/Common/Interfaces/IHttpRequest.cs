using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IHttpRequest
    {
        Task<string> GetRequest(string Uri);

        Task<HttpResponseMessage> GetHttpResp(string Uri);
    }
}
