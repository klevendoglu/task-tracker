using System.Web.Mvc;
using Abp.Dependency;
using Castle.Core.Internal;
using Mvc.Mailer;
using TaskTracker.ProjectManagement;

namespace TaskTracker.Mailers
{
    public sealed class ApplicationMailer : MailerBase, IApplicationMailer, ITransientDependency
    {
        public ApplicationMailer()
        {
            MasterName = "_Layout";
        }

        public MvcMailMessage PostOwnerNotified(NotifyPostOwnerOutput input, string recipient)
        {
            ViewData = new ViewDataDictionary(input);
            return Populate(x =>
            {
                x.Subject = input.Subject;
                x.ViewName = "PostOwnerNotified";
                x.To.Add(recipient);
                x.ReplyToList.Add(input.SenderEmail);
            });
        }

        public MvcMailMessage TaskManagerNotified(TaskListDto input, string recipients, string subject)
        {
            ViewData = new ViewDataDictionary(input);
            return Populate(x =>
            {
                x.Subject = subject;
                x.ViewName = "TaskManagerNotified";
                recipients.Split(',').ForEach(recipient => x.To.Add(recipient));
            });
        }

        public MvcMailMessage TaskManagerNotifiedForProject(ProjectWithManagersListDto input, string recipients, string subject)
        {
            ViewData = new ViewDataDictionary(input);
            return Populate(x =>
            {
                x.Subject = subject;
                x.ViewName = "TaskManagerNotifiedForProject";
                recipients.Split(',').ForEach(recipient => x.To.Add(recipient));
            });
        }

        public MvcMailMessage TaskAgentNotified(TaskListDto input, string recipients, string subject)
        {
            ViewData = new ViewDataDictionary(input);
            return Populate(x =>
            {
                x.Subject = subject;
                x.ViewName = "TaskAgentNotified";
                recipients.Split(',').ForEach(recipient => x.To.Add(recipient));
            });
        }
    }
}