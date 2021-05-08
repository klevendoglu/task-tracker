namespace TaskTracker.ProjectManagement
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Documents;

    [AutoMapFrom(typeof(TaskLogAttachment))]
    public class TaskLogAttachmentListDto : FullAuditedEntityDto
    {
        public virtual int TaskId { get; set; }

        public virtual int AttachmentId { get; set; }

        public virtual AttachmentListDto Attachment { get; set; }
    }
}