namespace TaskTracker.ProjectManagement
{
    using Abp.AutoMapper;

    [AutoMapFrom(typeof(TaskLog))]
    public class TaskLogWithTaskDto : TaskLogListDto
    {
        public int TaskId { get; set; }

        public TaskListDto ProjectTask { get; set; }
    }
}