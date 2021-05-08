using EntityFramework.DynamicFilters;
using TaskTracker.EntityFramework;

namespace TaskTracker.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly TaskTrackerDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(TaskTrackerDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();

            _context.SaveChanges();
        }
    }
}
