using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace Infrastructure
{
    public class HttpRequest : IHttpRequest
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> GetRequest(string Uri)
        {
            try
            {
                string result = await client.GetStringAsync(Uri);
                return result;
            }
            catch(Exception ex)
            {
                return "error: " + ex;
            }
        }

        public async Task<HttpResponseMessage> GetHttpResp(string Uri)
        {
            try
            {
                var result = await client.GetAsync(Uri);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
