using System.Collections.Generic;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTracker.Authorization.Roles;

namespace TaskTracker.Widgets
{
    [Table("Widgets", Schema = "Core")]
    public class Widget : Entity
    {
        [Required]
        public string Title { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public virtual ICollection<UserWidgets> UserWidgets { get; set; }
    }
}
