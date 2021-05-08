using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using TaskTracker.Timing.Dto;
using Abp.Auditing;

namespace TaskTracker.Timing
{
    [DisableAuditing]
    public class TimingAppService : TaskTrackerAppServiceBase, ITimingAppService
    {
        private readonly ITimeZoneService _timeZoneService;

        public TimingAppService(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }

        public async Task<ListResultOutput<NameValueDto>> GetTimezones(GetTimezonesInput input)
        {
            var timeZones = await GetTimezoneInfos(input.DefaultTimezoneScope);
            return new ListResultOutput<NameValueDto>(timeZones);
        }

        public async Task<List<ComboboxItemDto>> GetEditionComboboxItems(GetTimezoneComboboxItemsInput input)
        {
            var timeZones = await GetTimezoneInfos(input.DefaultTimezoneScope);
            var timeZoneItems = new ListResultOutput<ComboboxItemDto>(timeZones.Select(e => new ComboboxItemDto(e.Value, e.Name)).ToList()).Items.ToList();

            if (!string.IsNullOrEmpty(input.SelectedTimezoneId))
            {
                var selectedEdition = timeZoneItems.FirstOrDefault(e => e.Value == input.SelectedTimezoneId);
                if (selectedEdition != null)
                {
                    selectedEdition.IsSelected = true;
                }
            }

            return timeZoneItems;
        }

        private async Task<List<NameValueDto>> GetTimezoneInfos(SettingScopes defaultTimezoneScope)
        {
            var defaultTimezoneId = await _timeZoneService.GetDefaultTimezoneAsync(defaultTimezoneScope, AbpSession.TenantId);
            var defaultTimezone = TimeZoneInfo.FindSystemTimeZoneById(defaultTimezoneId);
            var defaultTimezoneName = $"{L("Default")} [{defaultTimezone.DisplayName}]";

            var timeZones = TimeZoneInfo.GetSystemTimeZones()
                                        .Select(tz => new NameValueDto(tz.DisplayName, tz.Id))
                                        .ToList();

            timeZones.Insert(0, new NameValueDto(defaultTimezoneName, string.Empty));
            return timeZones;
        }
    }
}