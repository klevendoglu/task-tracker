namespace TaskTracker.ProjectManagement
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Documents;

    [AutoMapFrom(typeof(ProjectTaskAttachment))]
    public class TaskAttachmentListDto : FullAuditedEntityDto
    {
        public virtual int TaskId { get; set; }

        public virtual int AttachmentId { get; set; }

        public virtual AttachmentListDto Attachment { get; set; }
    }
}