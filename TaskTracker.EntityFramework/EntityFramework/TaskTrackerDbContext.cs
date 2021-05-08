using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using TaskTracker.Authorization.Roles;
using TaskTracker.Authorization.Users;
using TaskTracker.MultiTenancy;
using TaskTracker.Storage;
using TaskTracker.ProjectManagement;
using TaskTracker.Documents;
using TaskTracker.Widgets;

namespace TaskTracker.EntityFramework
{
    public class TaskTrackerDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        /* Define an IDbSet for each entity of the application */

        #region Project Management

        public virtual IDbSet<Project> Projects { get; set; }

        public virtual IDbSet<ProjectAttachment> ProjectAttachments { get; set; }

        public virtual IDbSet<ProjectManager> ProjectManagers { get; set; }

        public virtual IDbSet<ProjectTask> Tasks { get; set; }
        public virtual IDbSet<ProjectTaskAttachment> TaskAttachments { get; set; }

        public virtual IDbSet<TaskLog> TaskLogs { get; set; }
        public virtual IDbSet<TaskLogAttachment> TaskLogAttachments { get; set; }
        public virtual IDbSet<ToDo> Todos { get; set; }

        public virtual IDbSet<Attachment> Attachments { get; set; }

        #endregion

        #region Widgets
        public virtual IDbSet<Widget> Widgets { get; set; }
        public virtual IDbSet<UserWidgets> UserWidgets { get; set; }
        #endregion

        public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }

        /* Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         * But it may cause problems when working Migrate.exe of EF. ABP works either way.         * 
         */
        public TaskTrackerDbContext()
            : base("Default")
        {
            
        }

        /* This constructor is used by ABP to pass connection string defined in TaskTrackerDataModule.PreInitialize.
         * Notice that, actually you will not directly create an instance of TaskTrackerDbContext since ABP automatically handles it.
         */
        public TaskTrackerDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /* This constructor is used in tests to pass a fake/mock connection.
         */
        public TaskTrackerDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {

        }
    }
}
