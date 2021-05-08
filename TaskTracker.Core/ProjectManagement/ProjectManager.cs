namespace TaskTracker.ProjectManagement
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Abp.Domain.Entities;

    using TaskTracker.Authorization.Users;

    [Table("ProjectManagers", Schema = "ProjectManagement")]
    public class ProjectManager : Entity
    {
        public virtual int ProjectId { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}