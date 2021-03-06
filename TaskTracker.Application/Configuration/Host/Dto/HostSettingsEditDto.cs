using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace TaskTracker.Configuration.Host.Dto
{
    public class HostSettingsEditDto : IDoubleWayDto
    {
        [Required]
        public GeneralSettingsEditDto General { get; set; }

        [Required]
        public HostUserManagementSettingsEditDto UserManagement { get; set; }

        [Required]
        public EmailSettingsEditDto Email { get; set; }

        [Required]
        public TenantManagementSettingsEditDto TenantManagement { get; set; }

    }
}