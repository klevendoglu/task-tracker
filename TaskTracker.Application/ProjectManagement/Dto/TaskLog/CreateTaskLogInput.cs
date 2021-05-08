namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Documents;

    [AutoMapTo(typeof(TaskLog))]
    public class CreateTaskLogInput : IInputDto
    {
        public int TaskId { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string TaskName { get; set; }

        [Required]
        [MaxLength(TaskLog.MaxNotesLength)]
        public string Notes { get; set; }

        public  IList<CreateAttachmentInput> Attachments { get; set; }

        public bool CloseTask { get; set; }
    }
}