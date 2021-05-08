using System.Collections.Generic;
using TaskTracker.Caching.Dto;

namespace TaskTracker.Web.Areas.Mpa.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}