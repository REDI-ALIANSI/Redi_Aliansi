using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.CONTENT.Command
{
    public class ShortenUrl : IRequest<string>
    {
        public string LongUrl { get; set; }
        public string Token { get; set; }
    }

    public class ShortenUrlHandler : IRequestHandler<ShortenUrl,string>
    {
        private readonly IShortenURL _ShortenUrl;

        public ShortenUrlHandler(IShortenURL shortenURL)
        {
            _ShortenUrl = shortenURL;
        }

        public Task<string> Handle(ShortenUrl request, CancellationToken cancellationToken)
        {
            var shortUrl = _ShortenUrl.Shorten(request.LongUrl, request.Token);

            return shortUrl;
        }
    }
}
