using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace TaskTracker.Authorization.Users.Widgets.Dto
{
    public class CreateUserWidgetsInput : IInputDto
    {
        public ICollection<int> WidgetIds { get; set; }
      
        [Required]
        public long UserId { get; set; }
    }
}
