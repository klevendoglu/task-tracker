namespace TaskTracker.ProjectManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Abp.Domain.Entities.Auditing;

    using TaskTracker.Authorization.Users;

    [Table("Projects", Schema = "ProjectManagement")]
    public class Project : FullAuditedEntity<int, User>
    {
        public const int MaxNameLength = 250;
        public const int MaxDescriptionLength = 1000;

        [MaxLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        public virtual TaskTracker.Enum.Status Status { get; set; }

        public virtual DateTime StartTime { get; set; }

        public virtual DateTime? EndTime { get; set; }

        public virtual DateTime? ClosingTime { get; set; }

        public virtual IList<ProjectTask> Tasks { get; set; }

        public virtual IList<ProjectManager> Managers { get; set; }

        public virtual IList<ProjectAttachment> ProjectAttachments { get; set; }
    }
}