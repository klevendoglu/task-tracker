namespace TaskTracker.ProjectManagement
{
    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    [AutoMapFrom(typeof(ToDo))]
    public class ToDoListDto : FullAuditedEntityDto
    {
        public string Title { get; set; }

        public bool IsComplete { get; set; }
    }
}