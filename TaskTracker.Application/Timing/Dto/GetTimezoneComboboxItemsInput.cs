using Abp.Application.Services.Dto;
using Abp.Configuration;

namespace TaskTracker.Timing.Dto
{
    public class GetTimezoneComboboxItemsInput : IInputDto
    {
        public SettingScopes DefaultTimezoneScope;

        public string SelectedTimezoneId { get; set; }
    }
}
