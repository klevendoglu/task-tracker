namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;

    using Abp.AutoMapper;

    [AutoMapFrom(typeof(ProjectTask))]
    public class TaskWithAttachmentsListDto : TaskListDto
    {
        public IList<TaskAttachmentListDto> TaskAttachments { get; set; }
    }
}