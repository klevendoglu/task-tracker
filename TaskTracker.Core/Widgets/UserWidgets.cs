using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using TaskTracker.Authorization.Users;

namespace TaskTracker.Widgets
{
    [Table("UserWidgets",Schema="Core")]
   public class UserWidgets : Entity
    {
        public long UserId { get; set; }
        public int WidgetId { get; set; }

        [ForeignKey("WidgetId")]
        public virtual  Widget Widget { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
