namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;
    using System.Linq;

    using Abp.AutoMapper;

    [AutoMapFrom(typeof(ProjectTask))]
    public class TaskWithProjectAndManagersListDto : TaskListDto
    {
        public ProjectListDto Project { get; set; }

        public IList<ProjectManagerListDto> Managers { get; set; }

        public long[] ManagerIds
        {
            get
            {
                return Managers.Select(manager => manager.UserId).ToArray();
            }
        }
       
    }
}