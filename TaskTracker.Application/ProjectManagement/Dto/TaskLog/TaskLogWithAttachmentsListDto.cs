namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;

    using Abp.AutoMapper;

    [AutoMapFrom(typeof(TaskLog))]
    public class TaskLogWithAttachmentsListDto : TaskLogListDto
    {
        public IList<TaskLogAttachmentListDto> TaskLogAttachments { get; set; }
    }
}