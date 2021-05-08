namespace TaskTracker
{
    using Abp.Application.Services.Dto;

    public class GetUserInput : IInputDto
    {
        public long? UserId { get; set; }

        public string IdentificationNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}
