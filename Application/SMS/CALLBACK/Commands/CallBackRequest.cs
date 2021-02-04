using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.CALLBACK.Commands
{
    public class CallBackRequest : IRequest<string>
    {
        public string rUrl { get; set; }
    }

    public class CallBackRequestHandler : IRequestHandler<CallBackRequest, string>
    {
        private readonly IHttpRequest _httpRequest;

        public CallBackRequestHandler(IHttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public async Task<string> Handle(CallBackRequest request, CancellationToken cancellationToken)
        {
            var resp = await _httpRequest.GetRequest(request.rUrl);

            return resp;
        }
    }
}
