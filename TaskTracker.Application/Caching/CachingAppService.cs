using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Caching;
using TaskTracker.Authorization;
using TaskTracker.Caching.Dto;
using Abp.Auditing;

namespace TaskTracker.Caching
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
    [DisableAuditing]
    public class CachingAppService : TaskTrackerAppServiceBase, ICachingAppService
    {
        private readonly ICacheManager _cacheManager;

        public CachingAppService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public ListResultOutput<CacheDto> GetAllCaches()
        {
            var caches = _cacheManager.GetAllCaches()
                                        .Select(cache => new CacheDto
                                        {
                                            Name = cache.Name
                                        })
                                        .ToList();

            return new ListResultOutput<CacheDto>(caches);
        }

        public async Task ClearCache(IdInput<string> input)
        {
            var cache = _cacheManager.GetCache(input.Id);
            await cache.ClearAsync();
        }

        public async Task ClearAllCaches()
        {
            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }
        }
    }
}