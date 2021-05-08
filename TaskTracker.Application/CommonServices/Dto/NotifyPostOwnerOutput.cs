namespace TaskTracker
{
    using Abp.Application.Services.Dto;

    public class NotifyPostOwnerOutput : IOutputDto
    {
        public string Subject { get; set; }

        public string Message { get; set; }

        public string Sender { get; set; }

        public string SenderEmail { get; set; }
    }
}
