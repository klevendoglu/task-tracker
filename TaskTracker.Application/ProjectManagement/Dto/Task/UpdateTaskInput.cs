namespace TaskTracker.ProjectManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Documents;

    [AutoMapTo(typeof(ProjectTask))]
    public class UpdateTaskInput : IInputDto
    {
        public int Id { get; set; }

        [MaxLength(ProjectTask.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(ProjectTask.MaxNameLength)]
        public string Name { get; set; }

        public int EstimatedDays { get; set; }

        public long? AgentId { get; set; }

        public int? ProjectId { get; set; }

        public DateTime? AssignTime { get; set; }

        public  IList<CreateAttachmentInput> Attachments { get; set; }
    }
}