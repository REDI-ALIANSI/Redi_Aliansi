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
            string result = await client.GetStringAsync(Uri);
            return result;
        }
    }
}
