using Abp.Application.Services.Dto;
using Abp.Configuration;

namespace TaskTracker.Timing.Dto
{
    public class GetTimezonesInput : IInputDto
    {
        public SettingScopes DefaultTimezoneScope;
    }
}
