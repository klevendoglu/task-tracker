namespace TaskTracker.ProjectManagement
{
    using Abp.Runtime.Validation;

    using Dto;

    public class GetTasksInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public int? CreatorUserId { get; set; }

        public int? ProjectId { get; set; }

        public int? AgentId { get; set; }

        public bool? IsOpen { get; set; }

        public bool OpenOnly { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(Sorting))
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}