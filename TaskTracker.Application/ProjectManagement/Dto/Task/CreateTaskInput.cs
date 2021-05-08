namespace TaskTracker.ProjectManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Documents;

    [AutoMapTo(typeof(ProjectTask))]
    public class CreateTaskInput : IInputDto
    {
        public CreateTaskInput()
        {
            Status = TaskTracker.Enum.Status.Open;
        }

        [MaxLength(ProjectTask.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(ProjectTask.MaxNameLength)]
        public string Name { get; set; }

        public int ProjectId { get; set; }

        public TaskTracker.Enum.Status Status { get; set; }

        public int EstimatedDays { get; set; }

        public long? AgentId { get; set; }

        public DateTime? AssignTime { get; set; }

        public  IList<CreateAttachmentInput> Attachments { get; set; }
    }
}