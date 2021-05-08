namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;
    using System.Linq;

    using Abp.AutoMapper;

    [AutoMapFrom(typeof(Project))]
    public class ProjectWithTasksAndManagersAndAttachmentsListDto : ProjectListDto
    {
        public IList<TaskListDto> Tasks { get; set; }

        public IList<ProjectManagerListDto> Managers { get; set; }
        
        public int TaskCount { get; set; }
        public string ManagersText
        {
            get
            {
                return Managers.Aggregate(string.Empty, (current, manager) => current + (manager.User.Surname + ", "));
            }
        }

        public long[] ManagerIds
        {
            get
            {
                return Managers.Select(manager => manager.UserId).ToArray();
            }
        }

        public IList<ProjectAttachmentListDto> ProjectAttachments { get; set; }
    }
}