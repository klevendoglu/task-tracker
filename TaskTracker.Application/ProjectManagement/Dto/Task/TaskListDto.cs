namespace TaskTracker.ProjectManagement
{
    using System;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Authorization.Users.Dto;

    [AutoMapFrom(typeof(ProjectTask))]
    public class TaskListDto : FullAuditedEntityDto
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public int ProjectId { get; set; }

        public TaskTracker.Enum.Status Status { get; set; }

        public int EstimatedDays { get; set; }

        public long? AgentId { get; set; }

        public DateTime? AssignTime { get; set; }

        public DateTime? ClosingTime { get; set; }

        public UserListDto CreatorUser { get; set; }

        public UserListDto AgentUser { get; set; }

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

        public int OverDue
        {
            get
            {
                if (AssignTime == null)
                {
                    return 0;
                }

                if (ClosingTime != null)
                {
                    return ((DateTime)ClosingTime - (DateTime)AssignTime.Value.AddDays(EstimatedDays)).Days;
                }

                return (DateTime.Today - (DateTime)AssignTime.Value.AddDays(EstimatedDays)).Days;
            }
        }
    }
}