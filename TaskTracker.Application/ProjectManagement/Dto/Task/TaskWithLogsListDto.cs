namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;

    using Abp.AutoMapper;

    [AutoMapFrom(typeof(ProjectTask))]
    public class TaskWithLogsListDto : TaskListDto
    {
        public ProjectListDto Project { get; set; }

        public IList<TaskLogListDto> TaskLogs { get; set; }

        public int TaskLogCount
        {
            get
            {
                return TaskLogs.Count;
            }
        }
    }
}