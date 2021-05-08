using TaskTracker.EntityFramework;

namespace TaskTracker.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly TaskTrackerDbContext _context;

        public InitialHostDbBuilder(TaskTrackerDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
