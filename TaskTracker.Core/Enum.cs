

namespace TaskTracker
{
    public class Enum
    {
        public enum Status
        {
            Pending, //0
            Approved,
            Rejected,
            Ready,
            Open,
            Assigned, //5
            Closed,
            InProcess,
            Processed,
            Finished,
            Requested, //10
            InUse,
            Available, //12
            Booked,
            Dropped,
            Withdrawn //15
        }

        public enum Role
        {
            Admin = 2,
            User = 3,
            Agent = 9,
            TaskManager = 4,
            TaskAgent = 5           
        }
    }
}
