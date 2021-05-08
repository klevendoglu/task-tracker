using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace TaskTracker.Localization.Dto
{
    public class CreateOrUpdateLanguageInput : IInputDto
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}