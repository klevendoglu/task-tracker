namespace TaskTracker.ProjectManagement
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Abp.Domain.Entities.Auditing;

    using Authorization.Users;

    [Table("ToDos", Schema = "ProjectManagement")]
    public class ToDo : FullAuditedEntity<int, User>
    {
        public const int MaxTitleLength = 500;
        
        [Required]
        [MaxLength(MaxTitleLength)]
        public virtual string Title { get; set; }

        public virtual bool IsComplete { get; set; }
    }
}