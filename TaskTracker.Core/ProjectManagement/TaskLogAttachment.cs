namespace TaskTracker.ProjectManagement
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Abp.Domain.Entities.Auditing;

    using TaskTracker.Authorization.Users;
    using TaskTracker.Documents;

    [Table("TaskLogAttachments", Schema = "ProjectManagement")]
    public class TaskLogAttachment : FullAuditedEntity<int, User>
    {
        public virtual int TaskLogId { get; set; }

        public virtual int AttachmentId { get; set; }

        [ForeignKey("TaskLogId")]
        public virtual TaskLog TaskLog { get; set; }

        [ForeignKey("AttachmentId")]
        public virtual Attachment Attachment { get; set; }
    }
}