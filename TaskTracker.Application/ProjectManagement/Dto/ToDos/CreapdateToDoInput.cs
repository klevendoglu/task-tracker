namespace TaskTracker.ProjectManagement
{
    using System.ComponentModel.DataAnnotations;

    using Abp.Application.Services.Dto;
    using Abp.AutoMapper;

    [AutoMapTo(typeof(ToDo))]
    public class CreapdateToDoInput : IInputDto
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(ToDo.MaxTitleLength)]
        public string Title { get; set; }

        public bool IsComplete { get; set; }
    }
}