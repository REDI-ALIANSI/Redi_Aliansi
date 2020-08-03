using Application.Common.Interfaces;
using Application.Common.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.BACKUP_PROCEDURES.Command
{
    public class BackupLiveTablesHandler : IRequestHandler<BackupLiveTables, Result>
    {
        private readonly IPostgreConnection _postgreConnection;

        public BackupLiveTablesHandler(IPostgreConnection postgreConnection)
        {
            _postgreConnection = postgreConnection;
        }

        public async Task<Result> Handle(BackupLiveTables request, CancellationToken cancellationToken)
        {
            try
            {
                //Call/RUN Procedure SMSOUT
                string queryCallSmsout = "CALL " + request.ProcedureSmsout + "();";
                await _postgreConnection.NonQuery(queryCallSmsout, request.Conn);

                //Call/RUN Procedure SMSIN
                string queryCallSmsin = "CALL " + request.ProcedureSmsin + "();";
                await _postgreConnection.NonQuery(queryCallSmsin, request.Conn);

                return Result.Success();
            }
            catch (Exception ex)
            {
                var errorIEnurable = new List<string>() { ex.Message };
                var resultExceltiop = Result.Failure(errorIEnurable);
                return resultExceltiop;
            }
        }
    }
}
