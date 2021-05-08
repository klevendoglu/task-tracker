using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TaskTracker.Configuration.Tenants.Dto;

namespace TaskTracker.Web.Areas.Mpa.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}