using System.Linq;
using TaskTracker.Editions;
using TaskTracker.EntityFramework;

namespace TaskTracker.Migrations.Seed.Tenants
{
    public class DefaultTenantBuilder
    {
        private readonly TaskTrackerDbContext _context;

        public DefaultTenantBuilder(TaskTrackerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDefaultTenant();
        }

        private void CreateDefaultTenant()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == MultiTenancy.Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new MultiTenancy.Tenant(MultiTenancy.Tenant.DefaultTenantName, MultiTenancy.Tenant.DefaultTenantName);

                var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    defaultTenant.EditionId = defaultEdition.Id;
                }

                _context.Tenants.Add(defaultTenant);
                _context.SaveChanges();
            }
        }
    }
}
