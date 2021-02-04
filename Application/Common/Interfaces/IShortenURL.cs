using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IShortenURL
    {
        public Task<string> Shorten(string LongUrl, string Token);
    }
}
