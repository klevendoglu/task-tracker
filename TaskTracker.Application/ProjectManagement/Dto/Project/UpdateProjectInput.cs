namespace TaskTracker.ProjectManagement
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;
    using Documents;
    using System.Collections.Generic;
    using Authorization.Users.Dto;

    [AutoMapTo(typeof(Project))]
    public class UpdateProjectInput : IInputDto
    {
        public int Id { get; set; }

        [MaxLength(Project.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(Project.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public DateTime? ClosingTime { get; set; }

        public TaskTracker.Enum.Status Status { get; set; }

        public int[] ManagerIds { get; set; }

        public IList<UserListDto> SelectedManagers { get; set; }

        public bool CloseProject { get; set; }

        public IList<CreateAttachmentInput> Attachments { get; set; }
    }
}