namespace TaskTracker.ProjectManagement
{
    using Abp.Runtime.Validation;

    using TaskTracker.Dto;

    public class GetProjectsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public int? CreatorUserId { get; set; }

        public int? ManagerId { get; set; }

        public bool OpenOnly { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(Sorting))
            {
                Sorting = "id, name,creatorUser,taskCount,startTime,endTime,status";
            }
        }
    }
}