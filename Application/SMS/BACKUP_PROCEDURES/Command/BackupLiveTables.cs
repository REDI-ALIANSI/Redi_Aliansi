using Application.Common.Model;
using MediatR;

namespace Application.SMS.BACKUP_PROCEDURES.Command
{
    public class BackupLiveTables : IRequest<Result>
    {
        public string ProcedureSmsout { get; set; }
        public string ProcedureSmsin { get; set; }
        public string Conn { get; set; }
    }
}
