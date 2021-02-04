using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Infrastructure
{
    public class Bitly_url : IShortenURL
    {
        private static string API_URL = "https://api-ssl.bit.ly/v4";
        public async Task<string> Shorten(string LongUrl, string Token)
        {
            var client = new RestClient(API_URL);
            var request = new RestRequest("shorten");
            request.AddHeader("Authorization", $"Bearer {Token}");
            var param = new Dictionary<string, string> {
                { "long_url", LongUrl }
            };
            request.AddJsonBody(param);
            var response = client.Post(request);
            string content = response.Content;
            JObject d = JObject.Parse(content);
            var result = (string)d["id"];
            return await Task.FromResult(result);
        }
    }
}
