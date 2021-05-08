using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTracker.Authorization.Users;

namespace TaskTracker.Documents
{
    [Table("Attachments", Schema = "Documents")]
    public class Attachment : FullAuditedEntity<int, User>
    {
        public virtual string Location { get; set; }

        public virtual string FileName { get; set; }

        public virtual string RefId { get; set; }
    }
}
