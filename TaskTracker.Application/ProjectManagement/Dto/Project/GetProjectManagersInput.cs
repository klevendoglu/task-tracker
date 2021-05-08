namespace TaskTracker.ProjectManagement
{
    using Abp.Runtime.Validation;
    using Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class GetProjectManagersInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int Id { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(Sorting))
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
