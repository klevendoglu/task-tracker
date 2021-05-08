namespace TaskTracker.ProjectManagement
{
    using System;

    public class TaskWithLogCountsListDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string CreatorUserName { get; set; }
        public string AgentUserName { get; set; }
        public int EstimatedDays { get; set; }
        public DateTime? AssignTime { get; set; }
        public DateTime? ClosingTime { get; set; }
        public int TaskLogCount { get; set; }
        public TaskTracker.Enum.Status Status { get; set; }

        public string StatusText
        {
            get
            {
                return Status.ToString();
            }
        }

        public int OverDue
        {
            get
            {
                if (AssignTime == null)
                {
                    return 0;
                }

                if (ClosingTime != null)
                {
                    return ((DateTime)ClosingTime - (DateTime)AssignTime.Value.AddDays(EstimatedDays)).Days;
                }

                return (DateTime.Today - (DateTime)AssignTime.Value.AddDays(EstimatedDays)).Days;
            }
        }

        public bool IsOpen
        {
            get
            {
                return Status == TaskTracker.Enum.Status.Open;
            }
        }

    }
}