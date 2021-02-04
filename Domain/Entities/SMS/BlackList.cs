using Domain.Common;

namespace Domain.Entities.SMS
{
    public class BlackList : AuditableEntity
    {
        public int BlackListId { get; set; }
        public string Msisdn { get; set; }
        public int OperatorId { get; set; }

        public Operator Operator { get; set; }
    }
}
