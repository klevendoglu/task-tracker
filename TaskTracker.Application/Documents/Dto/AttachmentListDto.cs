namespace TaskTracker.Documents
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    [AutoMapFrom(typeof(Attachment))]
    public class AttachmentListDto : FullAuditedEntityDto
    {
        public string Location { get; set; }

        public string FileName { get; set; }

        public string RefId { get; set; }

        public string Url => Location + RefId + "_" + FileName;
    }
}
