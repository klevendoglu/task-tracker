namespace TaskTracker.ProjectManagement
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Abp.Domain.Entities.Auditing;

    using TaskTracker.Authorization.Users;
    using TaskTracker.Documents;

    [Table("ProjectAttachments", Schema = "ProjectManagement")]
    public class ProjectAttachment : FullAuditedEntity<int, User>
    {
        public virtual int ProjectId { get; set; }

        public virtual int AttachmentId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("AttachmentId")]
        public virtual Attachment Attachment { get; set; }
    }
}