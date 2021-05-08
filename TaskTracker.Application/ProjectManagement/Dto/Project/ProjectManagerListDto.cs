namespace TaskTracker.ProjectManagement
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    using Authorization.Users.Dto;

    [AutoMapFrom(typeof(ProjectManager))]
    public class ProjectManagerListDto : EntityDto
    {
        public int ProjectId { get; set; }

        public long UserId { get; set; }

        public UserListDto User { get; set; }
    }
}