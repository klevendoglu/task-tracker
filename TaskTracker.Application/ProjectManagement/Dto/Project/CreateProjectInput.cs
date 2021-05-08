namespace TaskTracker.ProjectManagement
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;
    using Documents;
    using System.Collections.Generic;

    [AutoMapTo(typeof(Project))]
    public class CreateProjectInput : IInputDto
    {
        public CreateProjectInput()
        {
            Status = TaskTracker.Enum.Status.Open;
        }

        [MaxLength(Project.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(Project.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public long[] ManagerIds { get; set; }

        public TaskTracker.Enum.Status Status { get; set; }

        public IList<CreateAttachmentInput> Attachments { get; set; }
    }
}