namespace TaskTracker.ProjectManagement
{
    using Abp.Application.Services.Dto;

    public class GetToDosInput : IInputDto
    {
        public string Filter { get; set; }

        public int CreatorUserId { get; set; }

        public bool IsComplete { get; set; }
    }
}