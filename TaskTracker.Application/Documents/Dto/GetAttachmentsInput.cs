namespace TaskTracker.Documents
{
    using Abp.Runtime.Validation;
    using Dto;

    public class GetAttachmentsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public int OwnerId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(Sorting))
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
