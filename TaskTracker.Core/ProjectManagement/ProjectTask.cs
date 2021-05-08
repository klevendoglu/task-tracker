namespace TaskTracker.ProjectManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Abp.Domain.Entities.Auditing;

    using TaskTracker.Authorization.Users;

    [Table("Tasks", Schema = "ProjectManagement")]
    public class ProjectTask : FullAuditedEntity<int, User>
    {
        public const int MaxNameLength = 250;
        public const int MaxDescriptionLength = 1000;

        [MaxLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        public virtual int ProjectId { get; set; }

        public virtual TaskTracker.Enum.Status Status { get; set; }

        public virtual int EstimatedDays { get; set; }

        public virtual long? AgentId { get; set; }

        public virtual DateTime? AssignTime { get; set; }

        public virtual DateTime? ClosingTime { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("AgentId")]
        public virtual User AgentUser { get; set; }

        public virtual IList<TaskLog> TaskLogs { get; set; }

        public virtual IList<ProjectTaskAttachment> TaskAttachments { get; set; } 
    }
}