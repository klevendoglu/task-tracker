namespace TaskTracker.ProjectManagement
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Authorization.Users.Dto;

    [AutoMapFrom(typeof(TaskLog))]
    public class TaskLogListDto : FullAuditedEntityDto
    {
        public string Notes { get; set; }

        public UserListDto CreatorUser { get; set; }
    }
}