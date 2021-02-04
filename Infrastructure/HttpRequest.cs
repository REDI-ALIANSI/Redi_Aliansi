using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Newtonsoft.Json;

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

        public async Task<string> PostHttpResp(string Uri, object PostReq)
        {
            try
            {
                var Json = await Task.Run(() => JsonConvert.SerializeObject(PostReq,Formatting.Indented, 
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    }));
                var HttpContent = new StringContent(Json, Encoding.UTF8, "application/json");

                using(var httpClient = new HttpClient())
                {
                    var result = await httpClient.PostAsync(Uri, HttpContent);
                    return await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}