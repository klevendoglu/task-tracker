namespace TaskTracker.ProjectManagement
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Documents;

    [AutoMapFrom(typeof(ProjectAttachment))]
    public class ProjectAttachmentListDto : FullAuditedEntityDto
    {
        public virtual int ProjectId { get; set; }

        public virtual int AttachmentId { get; set; }

        public virtual AttachmentListDto Attachment { get; set; }
    }
}