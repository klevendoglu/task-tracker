using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using TaskTracker.Authorization.Users;

namespace TaskTracker.Configuration.Host.Dto
{
    public class SendTestEmailInput : IInputDto
    {
        [Required]
        [MaxLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}