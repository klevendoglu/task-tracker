namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;

    using Abp.AutoMapper;

    [AutoMapFrom(typeof(ProjectTask))]
    public class TaskWithAttachmentsAndLogsListDto : TaskListDto
    {
        public IList<TaskLogListDto> TaskLogs { get; set; }

        public IList<TaskAttachmentListDto> TaskAttachments { get; set; }

        public int TaskLogCount
        {
            get
            {
                return TaskLogs.Count;
            }
        }
    }
}