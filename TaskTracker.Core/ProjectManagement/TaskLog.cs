namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Abp.Domain.Entities.Auditing;

    using TaskTracker.Authorization.Users;

    [Table("TaskLogs", Schema = "ProjectManagement")]
    public class TaskLog : FullAuditedEntity<int, User>
    {
        public const int MaxNotesLength = 2500;
        
        public virtual int TaskId { get; set; }

        [Required]
        [MaxLength(MaxNotesLength)]
        public virtual string Notes { get; set; }
            
        [ForeignKey("TaskId")]
        public virtual ProjectTask ProjectTask { get; set; }

        public virtual IList<TaskLogAttachment> TaskLogAttachments { get; set; } 
    }
}