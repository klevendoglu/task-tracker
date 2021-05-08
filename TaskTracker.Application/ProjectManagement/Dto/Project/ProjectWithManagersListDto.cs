namespace TaskTracker.ProjectManagement
{
    using System;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Authorization.Users.Dto;
    using System.Collections.Generic;

    [AutoMapFrom(typeof(Project))]
    public class ProjectWithManagersListDto : FullAuditedEntityDto
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? ClosingTime { get; set; }

        public UserListDto CreatorUser { get; set; }

        public TaskTracker.Enum.Status Status { get; set; }

        public string StatusText
        {
            get
            {
                return Status.ToString();
            }
        }

        public bool IsOpen
        {
            get
            {
                return Status == TaskTracker.Enum.Status.Open;
            }
        }

        public bool IsClosed
        {
            get
            {
                return Status == TaskTracker.Enum.Status.Closed;
            }
        }

        public IList<ProjectManagerListDto> Managers { get; set; }
    }
}