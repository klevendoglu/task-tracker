namespace TaskTracker.Documents
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    [AutoMapTo(typeof(Attachment))]
    public class CreateAttachmentInput : IInputDto
    {
        public CreateAttachmentInput()
        {
            Location = "http://localhost:6240/Uploads/";
        }

        public string Location { get; set; }

        public string FileName { get; set; }

        public string RefId { get; set; }
    }
}
