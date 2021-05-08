using Mvc.Mailer;

namespace TaskTracker.Mailers
{

    using ProjectManagement;

    public interface IApplicationMailer
    {
        MvcMailMessage PostOwnerNotified(NotifyPostOwnerOutput input, string recipient);

        MvcMailMessage TaskManagerNotifiedForProject(ProjectWithManagersListDto input, string recipients, string subject);

        MvcMailMessage TaskManagerNotified(TaskListDto input, string recipients, string subject);

        MvcMailMessage TaskAgentNotified(TaskListDto input, string recipients, string subject);

    }
}