namespace TaskTracker.ProjectManagement
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Abp.Domain.Entities.Auditing;

    using TaskTracker.Authorization.Users;
    using TaskTracker.Documents;

    [Table("ProjectTaskAttachments", Schema = "ProjectManagement")]
    public class ProjectTaskAttachment : FullAuditedEntity<int, User>
    {
        public virtual int TaskId { get; set; }

        public virtual int AttachmentId { get; set; }

        [ForeignKey("TaskId")]
        public virtual ProjectTask ProjectTask { get; set; }

        [ForeignKey("AttachmentId")]
        public virtual Attachment Attachment { get; set; }
    }
}