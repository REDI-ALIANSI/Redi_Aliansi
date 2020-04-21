using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Application.Common.Behaviour
{
    public class CheckReservedKeywordHandler : IRequestHandler<CheckReservedKeyword, bool>
    {
        public Task<bool> Handle(CheckReservedKeyword request, CancellationToken cancellationToken)
        {
            List<string> reservedKeyword = new List<string> { "ON", "REG", "OFF", "UNREG", "HELP", "INFO" };
            string[] arrKeyword = request.Mo_Message.ToUpper().Split(' ');
            bool result = reservedKeyword.Any(str => str.Contains(arrKeyword[0]));

            return Task.FromResult(result);
        }
    }
}
