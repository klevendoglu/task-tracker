namespace TaskTracker.ProjectManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using Abp.AutoMapper;

    [AutoMapFrom(typeof(Project))]
    public class ProjectWithManagerAndTaskCountsOutput
    {
        public int Id { get; set; }
        public int TaskCount { get; set; }
        public IEnumerable<string> ManagersName { get; set; }
        public IEnumerable<string> ManagersSurname { get; set; }
        public string Name { get; set; }
        public string OwnersSurname { get; set; }
        public string CreatorUser { get; set; }
        public TaskTracker.Enum.Status Status { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public IEnumerable<int> ManagerIds { get; set; }
        //public string ManagersText
        //{
        //    get
        //    {
        //        return ManagersName.Aggregate(string.Empty, (current, manager) => current + (manager + ", "));
        //    }
        //}

        public string ManagersText
        {
            get
            {
                return ManagersSurname.Aggregate(string.Empty, (current, manager) => current + (manager + ", "));
            }
        }

        public string StartTimeText
        {
            get
            {
                return StartTime.ToShortDateString() + " " + StartTime.ToShortTimeString();
            }
        }

        public string EndTimeText
        {
            get
            {
                return EndTime != null ? ((DateTime)EndTime).ToShortDateString() + " " + ((DateTime)EndTime).ToShortTimeString() : string.Empty;
            }
        }

        public string StatusText
        {
            get
            {
                return Status.ToString();
            }
        }

        public bool IsOpen
        {
            get
            {
                return Status == TaskTracker.Enum.Status.Open;
            }
        }

        public bool IsTaskCountGreaterThanZero
        {
            get
            {
                return TaskCount > 0;
            }
        }
    }
}