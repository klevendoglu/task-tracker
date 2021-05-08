namespace TaskTracker.ProjectManagement
{
    using System.Threading.Tasks;

    using Abp.Application.Services;
    using Abp.Application.Services.Dto;

    using Authorization.Users.Dto;

    public interface IProjectManagementAppService : IApplicationService
    {
        #region Project

        Task CreateProject(CreateProjectInput input);
        Task UpdateProject(UpdateProjectInput input);
        Task DeleteProject(IdInput input);
        ProjectListDto GetProject(IdInput input);
        ProjectWithManagersListDto GetProjectWithManagers(IdInput input);
        ProjectWithTasksAndManagersListDto GetProjectWithTasksAndManagers(IdInput input);       
        ProjectWithTasksAndManagersAndAttachmentsListDto GetProjectWithTasksAndManagersAndAttachments(IdInput input);
        Task<PagedResultOutput<ProjectListDto>> GetProjects(GetProjectsInput input);
        Task<PagedResultOutput<ProjectWithTasksAndManagersListDto>> GetProjectsWithTasksAndManagers(GetProjectsInput input);
        Task<PagedResultOutput<ProjectWithManagerAndTaskCountsOutput>> GetProjectWithManagerAndTaskCounts(GetProjectsInput input);
        Task<ListResultOutput<UserListDto>> GetProjectAgents();
        Task<ListResultOutput<UserBasicListDto>> GetProjectManagers();
        Task<ListResultOutput<UserBasicListDto>> GetSelectedProjectManagers(GetProjectManagersInput input);
        Task<int[]> GetSelectedProjectManagersIds(GetProjectManagersInput input);
        ListResultOutput<ProjectAttachmentListDto> GetProjectAttachments(IdInput input);
        Task DeleteProjectAttachment(IdInput input);
        #endregion


        #region Task

        Task CreateTask(CreateTaskInput input);
        Task UpdateTask(UpdateTaskInput input);
        Task DeleteTask(IdInput input);
        TaskListDto GetTask(IdInput input);
        TaskWithProjectAndManagersListDto GetTaskWithProjectAndManagers(IdInput input);
        TaskWithAttachmentsListDto GetTaskWithAttachments(IdInput input);
        TaskWithAttachmentsAndLogsListDto GetTaskWithAttachmentsAndLogs(IdInput input);
        Task<PagedResultOutput<TaskListDto>> GetTasks(GetTasksInput input);
        Task<PagedResultOutput<TaskWithLogsListDto>> GetTasksWithLogs(GetTasksInput input);
        Task<PagedResultOutput<TaskWithLogCountsListDto>> GetTasksWithLogCounts(GetTasksInput input);
        Task<PagedResultOutput<TaskWithAttachmentsListDto>> GetTasksWithAttachments(GetTasksInput input);
        Task<PagedResultOutput<TaskWithAttachmentsAndLogsListDto>> GetTasksWithAttachmentsAndLogs(GetTasksInput input);
        ListResultOutput<TaskAttachmentListDto> GetTaskAttachments(IdInput input);
        Task DeleteTaskAttachment(IdInput input);
        Task PokeAgent(IdInput input);
        #endregion


        #region Task Log

        Task CreateTaskLog(CreateTaskLogInput input);
        Task UpdateTaskLog(UpdateTaskLogInput input);
        Task DeleteTaskLog(IdInput input);
        TaskLogListDto GetTaskLog(IdInput input);
        TaskLogWithTaskAndAttachmentsListDto GetTaskLogWithTaskAndAttachments(IdInput input);
        Task<PagedResultOutput<TaskLogListDto>> GetTaskLogs(GetTaskLogsInput input);
        Task<PagedResultOutput<TaskLogWithTaskAndAttachmentsListDto>> GetTaskLogsWithTaskAndAttachments(GetTaskLogsInput input);
        ListResultOutput<TaskLogAttachmentListDto> GetTaskLogAttachments(IdInput input);
        Task DeleteTaskLogAttachment(IdInput input);

        #endregion

        #region ToDos

        ToDoListDto GetToDo(IdInput input);

        ListResultOutput<ToDoListDto> GetToDos(GetToDosInput input);

        Task CreapdateToDo(CreapdateToDoInput input);

        Task MarkToDoComplete(IdInput input);

        Task DeleteToDo(IdInput input);

        #endregion
    }
}
