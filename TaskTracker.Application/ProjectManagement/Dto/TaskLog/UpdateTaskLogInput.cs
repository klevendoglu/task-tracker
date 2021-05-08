namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Abp.Application.Services.Dto;

    using Documents;
    using Abp.AutoMapper;

    [AutoMapTo(typeof(TaskLog))]
    public class UpdateTaskLogInput : IInputDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(TaskLog.MaxNotesLength)]
        public string Notes { get; set; }

        public IList<CreateAttachmentInput> Attachments { get; set; }

        public bool CloseTask { get; set; }
    }
}