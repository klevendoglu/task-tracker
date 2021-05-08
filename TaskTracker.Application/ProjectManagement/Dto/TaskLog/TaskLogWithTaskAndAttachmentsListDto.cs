namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;

    using Abp.AutoMapper;

    [AutoMapFrom(typeof(TaskLog))]
    public class TaskLogWithTaskAndAttachmentsListDto : TaskLogListDto
    {
        public int TaskId { get; set; }

        public TaskListDto ProjectTask { get; set; }

        public IList<TaskLogAttachmentListDto> TaskLogAttachments { get; set; }
    }
}