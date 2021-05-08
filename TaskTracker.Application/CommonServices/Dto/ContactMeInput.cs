namespace TaskTracker
{
    using System.ComponentModel.DataAnnotations;

    using Abp.Application.Services.Dto;

    public class ContactMeInput : IInputDto
    {
        public int SenderId { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public int PostOwnerId { get; set; }
    }
}
