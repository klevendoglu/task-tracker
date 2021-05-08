namespace TaskTracker.ProjectManagement
{
    using Abp.Runtime.Validation;

    using Dto;

    public class GetTaskLogsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public int? CreatorUserId { get; set; }

        public int? TaskId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(Sorting))
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}