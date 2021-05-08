namespace TaskTracker.ProjectManagement
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;

    using Abp.Application.Services.Dto;
    using Abp.Authorization;
    using Abp.AutoMapper;
    using Abp.Domain.Repositories;
    using Abp.Linq.Extensions;
    using Abp.Extensions;

    using Authorization.Users.Dto;
    using Documents;
    using Mailers;
    using Authorization.Users;
    using Abp.UI;

    using Authorization;
    using Abp.Auditing;
    using Notifications;

    [AbpAuthorize]
    [DisableAuditing]
    public class ProjectManagementAppService : TaskTrackerAppServiceBase, IProjectManagementAppService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<ProjectAttachment> _projectAttachmentRepository;
        private readonly IRepository<ProjectManager> _projectManagerRepository;
        private readonly IRepository<ProjectTask> _taskRepository;
        private readonly IRepository<ProjectTaskAttachment> _taskAttachmentRepository;
        private readonly IRepository<TaskLog> _taskLogRepository;
        private readonly IRepository<TaskLogAttachment> _taskLogAttachmentRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<ToDo> _todoRepository;
        private readonly UserManager _userManager;

        private readonly IAppNotifier _appNotifier;


        private readonly IApplicationMailer mailer = new ApplicationMailer();

        public ProjectManagementAppService(IRepository<Project> projectRepository, IRepository<ProjectAttachment> projectAttachmentRepository, IRepository<ProjectManager> projectManagerRepository,
            IRepository<ProjectTask> taskRepository, IRepository<TaskLog> taskLogRepository, IRepository<Attachment> attachmentRepository,
            IRepository<ProjectTaskAttachment> taskAttachmentRepository, IRepository<TaskLogAttachment> taskLogAttachmentRepository,
            UserManager userManager, IRepository<ToDo> todoRepository, IAppNotifier appNotifier)
        {
            _projectRepository = projectRepository;
            _projectAttachmentRepository = projectAttachmentRepository;
            _projectManagerRepository = projectManagerRepository;
            _taskRepository = taskRepository;
            _taskLogRepository = taskLogRepository;
            _attachmentRepository = attachmentRepository;
            _taskAttachmentRepository = taskAttachmentRepository;
            _taskLogAttachmentRepository = taskLogAttachmentRepository;
            _userManager = userManager;
            _todoRepository = todoRepository;

            _appNotifier = appNotifier;
        }

        #region Project

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task CreateProject(CreateProjectInput input)
        {
            var toCreate = input.MapTo<Project>();
            var projectId = await _projectRepository.InsertAndGetIdAsync(toCreate);
            if (input.Attachments.Count > 0)
            {
                foreach (CreateAttachmentInput attachment in input.Attachments)
                {
                    var attachmentId = await _attachmentRepository.InsertAndGetIdAsync(attachment.MapTo<Attachment>());
                    await _projectAttachmentRepository.InsertAsync(new ProjectAttachment { ProjectId = projectId, AttachmentId = attachmentId });
                }
            }
            foreach (var managerId in input.ManagerIds)
            {
                await _projectManagerRepository.InsertAsync(new ProjectManager { ProjectId = projectId, UserId = managerId });
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task UpdateProject(UpdateProjectInput input)
        {
            var toUpdate = await _projectRepository.GetAsync(input.Id);
            // update project
            input.Status = input.CloseProject ? TaskTracker.Enum.Status.Closed : TaskTracker.Enum.Status.Open;
            input.ClosingTime = input.CloseProject ? DateTime.Now : (DateTime?)null;
            
            input.MapTo(toUpdate);
            if (input.Attachments.Count > 0)
            {
                foreach (CreateAttachmentInput attachment in input.Attachments)
                {
                    var attachmentId = await _attachmentRepository.InsertAndGetIdAsync(attachment.MapTo<Attachment>());
                    await _projectAttachmentRepository.InsertAsync(new ProjectAttachment { ProjectId = input.Id, AttachmentId = attachmentId });
                }
            }
            // delete old project managers and create the new ones
            await _projectManagerRepository.DeleteAsync(x => x.ProjectId == input.Id);
            foreach (var manager in input.SelectedManagers)
            {
                await _projectManagerRepository.InsertAsync(new ProjectManager { ProjectId = input.Id, UserId = manager.Id });
            }

            CurrentUnitOfWork.SaveChanges();

            if (input.CloseProject)
            {
                var projectWithManagers = GetProjectWithManagers(new IdInput() { Id = input.Id });
                NotifyManagersForProject(projectWithManagers, @L("ProjectClosed"));

                //notification 
                var managerIds = await GetSelectedProjectManagersIds(new GetProjectManagersInput() { Id = input.Id });
                var closerUser = GetCurrentUser().FullName;
                foreach (var managerId in managerIds)
                {
                    await _appNotifier.NewTaskClosedEventAsync(closerUser + " kullanıcısı " + input.Name + " projesini kapattı.", input.Name, new Abp.UserIdentifier(AbpSession.TenantId, managerId));
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task DeleteProject(IdInput input)
        {
            await _projectRepository.DeleteAsync(input.Id);
        }

        public ProjectListDto GetProject(IdInput input)
        {
            var toReturn = _projectRepository.Get(input.Id);
            return toReturn.MapTo<ProjectListDto>();
        }

        public ListResultOutput<ProjectAttachmentListDto> GetProjectAttachments(IdInput input)
        {
            var toReturn = _projectAttachmentRepository.GetAll().Include(x => x.Attachment).Where(x => x.ProjectId == input.Id).ToList();
            return new ListResultOutput<ProjectAttachmentListDto>(toReturn.MapTo<List<ProjectAttachmentListDto>>());
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task DeleteProjectAttachment(IdInput input)
        {
            await _projectAttachmentRepository.DeleteAsync(input.Id);
        }

        public ProjectWithTasksAndManagersListDto GetProjectWithTasksAndManagers(IdInput input)
        {
            var toReturn =
                _projectRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.Tasks)
                    .Include("Tasks.CreatorUser")
                    .Include("Tasks.AgentUser")
                    .Include(p => p.Managers)
                    .Include("Managers.User")
                    .FirstOrDefault(x => x.Id == input.Id);
            return toReturn.MapTo<ProjectWithTasksAndManagersListDto>();
        }

        public ProjectWithManagersListDto GetProjectWithManagers(IdInput input)
        {
            var toReturn =
                _projectRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.Managers)
                    .Include("Managers.User")
                    .FirstOrDefault(x => x.Id == input.Id);
            return toReturn.MapTo<ProjectWithManagersListDto>();
        }

        public ProjectWithTasksAndManagersAndAttachmentsListDto GetProjectWithTasksAndManagersAndAttachments(IdInput input)
        {
            var toReturn =
                _projectRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.Tasks)
                    .Include("Tasks.CreatorUser")
                    .Include("Tasks.AgentUser")
                    .Include(p => p.Managers)
                    .Include("Managers.User")
                    .Include(p => p.ProjectAttachments)
                    .Include("ProjectAttachments.Attachment")
                    .FirstOrDefault(x => x.Id == input.Id);
            return toReturn.MapTo<ProjectWithTasksAndManagersAndAttachmentsListDto>();
        }

        public async Task<PagedResultOutput<ProjectListDto>> GetProjects(GetProjectsInput input)
        {
            var query =
                _projectRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .WhereIf(input.CreatorUserId != null, t => t.CreatorUserId == input.CreatorUserId)
                    .WhereIf(input.ManagerId != null, t => t.Managers.Any(x => x.UserId == input.ManagerId))
                    .WhereIf(
                        !string.IsNullOrEmpty(input.Filter),
                        t => t.Name.Contains(input.Filter) || t.Description.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var projects = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var toReturn = projects.MapTo<List<ProjectListDto>>();
            return new PagedResultOutput<ProjectListDto>(totalCount, toReturn);
        }

        public async Task<PagedResultOutput<ProjectWithTasksAndManagersListDto>> GetProjectsWithTasksAndManagers(GetProjectsInput input)
        {
            var query =
                _projectRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.Tasks)
                    .Include("Tasks.CreatorUser")
                    .Include("Tasks.AgentUser")
                    .Include(p => p.Managers)
                    .Include("Managers.User")
                    .WhereIf(input.CreatorUserId != null, t => t.CreatorUserId == input.CreatorUserId)
                    .WhereIf(input.ManagerId != null, t => t.Managers.Any(x => x.UserId == input.ManagerId))
                    .WhereIf(
                        !string.IsNullOrEmpty(input.Filter),
                        t => t.Name.Contains(input.Filter) || t.Description.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var projects = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var toReturn = projects.MapTo<List<ProjectWithTasksAndManagersListDto>>();
            return new PagedResultOutput<ProjectWithTasksAndManagersListDto>(totalCount, toReturn);
        }

        public async Task<PagedResultOutput<ProjectWithManagerAndTaskCountsOutput>> GetProjectWithManagerAndTaskCounts(GetProjectsInput input)
        {
            try
            {
                var result = (from project in _projectRepository.GetAll()
                                    .WhereIf(input.CreatorUserId != null, x => x.CreatorUserId == input.CreatorUserId)
                                    .WhereIf(input.ManagerId != null, x => x.Managers.Any(t => t.UserId == input.ManagerId))
                                    .WhereIf(input.OpenOnly, t => t.Status != TaskTracker.Enum.Status.Closed)
                                    .WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Name.Contains(input.Filter))
                              join task in _taskRepository.GetAll() on project.Id equals task.ProjectId into projectTasks
                              select
                                  new ProjectWithManagerAndTaskCountsOutput
                                  {
                                      Id = project.Id,
                                      StartTime = project.StartTime,
                                      EndTime = project.EndTime,
                                      Name = project.Name,
                                      Status = project.Status,
                                      CreatorUser = project.CreatorUser.Name + " " + project.CreatorUser.Surname,
                                      OwnersSurname = project.CreatorUser.Surname,
                                      CreatorUserId = project.CreatorUserId,
                                      ManagerIds = project.Managers.Select(x => x.Id),
                                      ManagersName = project.Managers.Select(x => x.User.UserName),
                                      ManagersSurname = project.Managers.Select(x => x.User.Surname),
                                      TaskCount = project.Tasks.Count
                                  });
                var totalCount = result.Count();
                var tasks = await result.OrderBy(input.Sorting).PageBy(input).ToListAsync();

                return new PagedResultOutput<ProjectWithManagerAndTaskCountsOutput>(totalCount, tasks);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<ListResultOutput<UserListDto>> GetProjectAgents()
        {
            return await CommonServices.GetUsersByRole(TaskTracker.Enum.Role.TaskAgent);
        }

        public async Task<ListResultOutput<UserBasicListDto>> GetProjectManagers()
        {
            return await CommonServices.GetBasicUsersByRole(TaskTracker.Enum.Role.TaskManager);
        }

        public async Task<ListResultOutput<UserBasicListDto>> GetSelectedProjectManagers(GetProjectManagersInput input)
        {
            var toReturn = await _projectRepository.GetAll()
                .Include(x => x.Managers)
                .Where(r => r.Id == input.Id).SelectMany(t => t.Managers.Select(y => new UserBasicListDto()
                {
                    Id = y.User.Id,
                    Name = y.User.Name,
                    Surname = y.User.Surname,
                    EmailAddress = y.User.EmailAddress,
                    UserName = y.User.UserName,
                    IsActive = y.User.IsActive,
                    CreationTime = y.User.CreationTime,
                    IsEmailConfirmed = y.User.IsEmailConfirmed,
                    LastLoginTime = y.User.LastLoginTime,
                    ProfilePictureId = y.User.ProfilePictureId
                })).ToListAsync();
            return new ListResultOutput<UserBasicListDto>(toReturn.MapTo<List<UserBasicListDto>>());
        }

        public async Task<int[]> GetSelectedProjectManagersIds(GetProjectManagersInput input)
        {
            var query = await (from project in _projectRepository.GetAll()
                               where project.Id == input.Id
                               join projectManager in _projectManagerRepository.GetAll() on project.Id equals projectManager.ProjectId into projectWithManagerIds
                               from pwmIds in projectWithManagerIds
                               select (int)pwmIds.UserId).DefaultIfEmpty(0).ToArrayAsync();
            return query;
        }

        #endregion

        #region Task

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task CreateTask(CreateTaskInput input)
        {
            input.AssignTime = input.AgentId == null ? (DateTime?)null : DateTime.Now;
            var toCreate = input.MapTo<ProjectTask>();
            var taskId = await _taskRepository.InsertAndGetIdAsync(toCreate);
            if (input.Attachments.Count > 0)
            {
                foreach (CreateAttachmentInput attachment in input.Attachments)
                {
                    var attachmentId = await _attachmentRepository.InsertAndGetIdAsync(attachment.MapTo<Attachment>());
                    await _taskAttachmentRepository.InsertAsync(new ProjectTaskAttachment { TaskId = taskId, AttachmentId = attachmentId });
                }
            }
            if (input.AgentId != null)
            {
                //await NotifyAgent(new TaskListDto { AgentId = input.AgentId, Name = input.Name, Description = input.Description, EstimatedDays = input.EstimatedDays }, @L("TaskAssignedToYou"));


                //notification
                var currentUser = GetCurrentUser();
                await _appNotifier.NewTaskAssignedEventAsync(currentUser.FullName + " size " + input.Name + " görevini atadı.", input.Name, new Abp.UserIdentifier(AbpSession.TenantId, (long)input.AgentId));
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task UpdateTask(UpdateTaskInput input)
        {
            var toUpdate = await _taskRepository.GetAsync(input.Id);
            input.AssignTime = input.AgentId == null ? null : toUpdate.AgentId == null ? DateTime.Now : toUpdate.AssignTime;
            input.MapTo(toUpdate);
            if (input.Attachments.Count > 0)
            {
                foreach (CreateAttachmentInput attachment in input.Attachments)
                {
                    var attachmentId = await _attachmentRepository.InsertAndGetIdAsync(attachment.MapTo<Attachment>());
                    await _taskAttachmentRepository.InsertAsync(new ProjectTaskAttachment { TaskId = input.Id, AttachmentId = attachmentId });
                }
            }
            if (input.AgentId != null)
            {
                await NotifyAgent(new TaskListDto { AgentId = input.AgentId, Name = input.Name, Description = input.Description, EstimatedDays = input.EstimatedDays }, @L("TaskAssignedToYou"));
                //notification
                var currentUser = GetCurrentUser();
                await _appNotifier.NewTaskAssignedEventAsync(currentUser.FullName + " size " + input.Name + " görevini atadı.", input.Name, new Abp.UserIdentifier(AbpSession.TenantId, (long)input.AgentId));
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task DeleteTask(IdInput input)
        {
            await _taskRepository.DeleteAsync(input.Id);
        }

        public TaskListDto GetTask(IdInput input)
        {
            var toReturn = _taskRepository.Get(input.Id);
            return toReturn.MapTo<TaskListDto>();
        }

        public TaskWithAttachmentsListDto GetTaskWithAttachments(IdInput input)
        {
            var toReturn = _taskRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.AgentUser)
                    .Include("TaskLogs.CreatorUser")
                    .Include(p => p.TaskAttachments)
                    .Include("TaskAttachments.Attachment")
                    .FirstOrDefault(x => x.Id == input.Id);
            return toReturn.MapTo<TaskWithAttachmentsListDto>();
        }

        public TaskWithProjectAndManagersListDto GetTaskWithProjectAndManagers(IdInput input)
        {
            var toReturn = _taskRepository.GetAll()
                    .Include(p => p.Project)
                    .Include("Project.Managers")
                    .Include("Managers.User")
                    .FirstOrDefault(x => x.Id == input.Id);
            return toReturn.MapTo<TaskWithProjectAndManagersListDto>();
        }

        public TaskWithAttachmentsAndLogsListDto GetTaskWithAttachmentsAndLogs(IdInput input)
        {
            var toReturn = _taskRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.AgentUser)
                    .Include(p => p.TaskLogs)
                    .Include("TaskLogs.CreatorUser")
                    .Include(p => p.TaskAttachments)
                    .Include("TaskAttachments.Attachment")
                    .FirstOrDefault(x => x.Id == input.Id);

            return toReturn.MapTo<TaskWithAttachmentsAndLogsListDto>();
        }

        public async Task<PagedResultOutput<TaskListDto>> GetTasks(GetTasksInput input)
        {
            var query =
                _taskRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.AgentUser)
                    .WhereIf(input.CreatorUserId != null, t => t.CreatorUserId == input.CreatorUserId)
                    .WhereIf(input.ProjectId != null, t => t.ProjectId == input.ProjectId)
                    .WhereIf(input.AgentId != null, t => t.AgentId == input.AgentId)
                    .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Name.Contains(input.Filter) || t.Description.Contains(input.Filter) || t.AgentUser.Surname.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var tasks = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var toReturn = tasks.MapTo<List<TaskListDto>>();
            return new PagedResultOutput<TaskListDto>(totalCount, toReturn);
        }

        public async Task<PagedResultOutput<TaskWithLogsListDto>> GetTasksWithLogs(GetTasksInput input)
        {
            var query =
                _taskRepository.GetAll()
                    .Include(p => p.Project)
                    .Include(p => p.CreatorUser)
                    .Include(p => p.AgentUser)
                    .Include(p => p.TaskLogs)
                    .Include("TaskLogs.CreatorUser")
                    .WhereIf(input.CreatorUserId != null, t => t.CreatorUserId == input.CreatorUserId)
                    .WhereIf(input.ProjectId != null, t => t.ProjectId == input.ProjectId)
                    .WhereIf(input.AgentId != null, t => t.AgentId == input.AgentId)
                    .WhereIf(input.OpenOnly, t => t.Status != TaskTracker.Enum.Status.Closed)
                    .WhereIf(input.IsOpen != null, t => t.Status == ((bool)input.IsOpen ? TaskTracker.Enum.Status.Open : TaskTracker.Enum.Status.Closed))
                    .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Name.Contains(input.Filter) || t.Description.Contains(input.Filter) || t.AgentUser.Surname.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var tasks = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var toReturn = tasks.MapTo<List<TaskWithLogsListDto>>();
            return new PagedResultOutput<TaskWithLogsListDto>(totalCount, toReturn);
        }

        public async Task<PagedResultOutput<TaskWithLogCountsListDto>> GetTasksWithLogCounts(GetTasksInput input)
        {
            var result = await (from task in _taskRepository.GetAll().WhereIf(input.ProjectId.HasValue, x => x.ProjectId == input.ProjectId.Value)
                                                                          .WhereIf(input.CreatorUserId.HasValue, x => x.CreatorUserId == input.CreatorUserId)
                                                                          .WhereIf(input.AgentId.HasValue, x => x.AgentId == input.AgentId)
                                                                          .WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Name.Contains(input.Filter))
                                join log in _taskLogRepository.GetAll() on task.Id equals log.TaskId into taskLogs
                                select new TaskWithLogCountsListDto
                                {
                                    Id = task.Id,
                                    TaskName = task.Name,
                                    AssignTime = task.AssignTime,
                                    ClosingTime = task.ClosingTime,
                                    CreatorUserName = task.CreatorUser.Name + " " + task.CreatorUser.Surname,
                                    EstimatedDays = task.EstimatedDays,
                                    ProjectName = task.Project.Name,
                                    Status = task.Status,
                                    AgentUserName = task.AgentUser.Name + " " + task.AgentUser.Surname,
                                    TaskLogCount = taskLogs.Count(),
                                }).ToListAsync();

            return new PagedResultOutput<TaskWithLogCountsListDto>(result.Count, result);
        }

        public async Task<PagedResultOutput<TaskWithAttachmentsListDto>> GetTasksWithAttachments(GetTasksInput input)
        {
            var query =
                _taskRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.AgentUser)
                    .Include(p => p.TaskAttachments)
                    .Include("TaskAttachments.Attachment")
                    .WhereIf(input.CreatorUserId != null, t => t.CreatorUserId == input.CreatorUserId)
                    .WhereIf(input.ProjectId != null, t => t.ProjectId == input.ProjectId)
                    .WhereIf(input.AgentId != null, t => t.AgentId == input.AgentId)
                    .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Name.Contains(input.Filter) || t.Description.Contains(input.Filter) || t.AgentUser.Surname.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var tasks = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var toReturn = tasks.MapTo<List<TaskWithAttachmentsListDto>>();
            return new PagedResultOutput<TaskWithAttachmentsListDto>(totalCount, toReturn);
        }

        public async Task<PagedResultOutput<TaskWithAttachmentsAndLogsListDto>> GetTasksWithAttachmentsAndLogs(GetTasksInput input)
        {
            var query =
                _taskRepository.GetAll()
                    .Include(p => p.TaskLogs)
                    .Include("TaskLogs.CreatorUser")
                    .Include(p => p.TaskAttachments)
                    .Include("TaskAttachments.Attachment")
                    .Include(p => p.CreatorUser)
                    .Include(p => p.AgentUser)
                    .WhereIf(input.CreatorUserId != null, t => t.CreatorUserId == input.CreatorUserId)
                    .WhereIf(input.ProjectId != null, t => t.ProjectId == input.ProjectId)
                    .WhereIf(input.AgentId != null, t => t.AgentId == input.AgentId)
                    .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Name.Contains(input.Filter) || t.Description.Contains(input.Filter) || t.AgentUser.Surname.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var tasks = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var toReturn = tasks.MapTo<List<TaskWithAttachmentsAndLogsListDto>>();
            return new PagedResultOutput<TaskWithAttachmentsAndLogsListDto>(totalCount, toReturn);
        }

        public ListResultOutput<TaskAttachmentListDto> GetTaskAttachments(IdInput input)
        {
            var toReturn = _taskAttachmentRepository.GetAll().Include(x => x.Attachment).Where(x => x.TaskId == input.Id).ToList();
            return new ListResultOutput<TaskAttachmentListDto>(toReturn.MapTo<List<TaskAttachmentListDto>>());
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task DeleteTaskAttachment(IdInput input)
        {
            await _taskAttachmentRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        public async Task PokeAgent(IdInput input)
        {
            await NotifyAgent(GetTask(new IdInput { Id = input.Id }), @L("PokedAgent"));
        }

        #endregion

        #region Task Log

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Agent)]
        public async Task CreateTaskLog(CreateTaskLogInput input)
        {
            var toCreate = input.MapTo<TaskLog>();
            if (input.CloseTask)
            {
                var toClose = _taskRepository.Get(toCreate.TaskId);
                toClose.Status = TaskTracker.Enum.Status.Closed;
                toClose.ClosingTime = DateTime.Now;
                NotifyManagers(toClose, @L("TaskClosed"));

                //notification 
                var managerIds = await GetSelectedProjectManagersIds(new GetProjectManagersInput() { Id = toClose.ProjectId });
                foreach (var managerId in managerIds)
                {
                   await _appNotifier.NewTaskClosedEventAsync(toClose.AgentUser.FullName + " kullanıcısı " + toClose.Name + " görevini tamamladı.", toClose.Name, new Abp.UserIdentifier(AbpSession.TenantId, managerId));
                }             
            }

            var taskLogId = await _taskLogRepository.InsertAndGetIdAsync(toCreate);

            if (input.Attachments.Count == 0)
            {
                return;
            }

            foreach (CreateAttachmentInput attachment in input.Attachments)
            {
                var attachmentId = await _attachmentRepository.InsertAndGetIdAsync(attachment.MapTo<Attachment>());
                await _taskLogAttachmentRepository.InsertAsync(new TaskLogAttachment { TaskLogId = taskLogId, AttachmentId = attachmentId });
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Agent)]
        public async Task UpdateTaskLog(UpdateTaskLogInput input)
        {
            var toUpdate = await _taskLogRepository.GetAsync(input.Id);
            input.MapTo(toUpdate);
            var toClose = await _taskRepository.GetAsync(toUpdate.TaskId);
            if (input.CloseTask)
            {
                toClose.Status = TaskTracker.Enum.Status.Closed;
                toClose.ClosingTime = DateTime.Now;
                NotifyManagers(toClose, @L("TaskClosed"));

                //notification 
                var managerIds = await GetSelectedProjectManagersIds(new GetProjectManagersInput() { Id = toClose.ProjectId });
                foreach (var managerId in managerIds)
                {
                    await _appNotifier.NewTaskClosedEventAsync(toClose.AgentUser.FullName + " kullanıcısı " + toClose.Name + " görevini tamamladı.", toClose.Name, new Abp.UserIdentifier(AbpSession.TenantId, managerId));
                }
            }
            else
            {
                toClose.Status = TaskTracker.Enum.Status.Open;
            }
            if (input.Attachments.Count == 0)
            {
                return;
            }

            foreach (CreateAttachmentInput attachment in input.Attachments)
            {
                var attachmentId = await _attachmentRepository.InsertAndGetIdAsync(attachment.MapTo<Attachment>());
                await _taskLogAttachmentRepository.InsertAsync(new TaskLogAttachment { TaskLogId = input.Id, AttachmentId = attachmentId });
            }
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Agent)]
        public async Task DeleteTaskLog(IdInput input)
        {
            await _taskLogRepository.DeleteAsync(input.Id);
        }

        public TaskLogListDto GetTaskLog(IdInput input)
        {
            var toReturn = _taskLogRepository.Get(input.Id);
            return toReturn.MapTo<TaskLogListDto>();
        }

        public TaskLogWithTaskAndAttachmentsListDto GetTaskLogWithTaskAndAttachments(IdInput input)
        {
            var toReturn =
                _taskLogRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.ProjectTask)
                    .Include("ProjectTask.CreatorUser")
                    .Include("ProjectTask.AgentUser")
                    .Include(p => p.TaskLogAttachments)
                    .Include("TaskLogAttachments.Attachment")
                    .FirstOrDefault(x => x.Id == input.Id);

            return toReturn.MapTo<TaskLogWithTaskAndAttachmentsListDto>();
        }

        public async Task<PagedResultOutput<TaskLogListDto>> GetTaskLogs(GetTaskLogsInput input)
        {
            var query =
                _taskLogRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .WhereIf(input.CreatorUserId != null, t => t.CreatorUserId == input.CreatorUserId)
                    .WhereIf(input.TaskId != null, t => t.TaskId == input.TaskId)
                    .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Notes.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var taskLogs = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var toReturn = taskLogs.MapTo<List<TaskLogListDto>>();
            return new PagedResultOutput<TaskLogListDto>(totalCount, toReturn);
        }

        public async Task<PagedResultOutput<TaskLogWithTaskAndAttachmentsListDto>> GetTaskLogsWithTaskAndAttachments(GetTaskLogsInput input)
        {
            var query =
                _taskLogRepository.GetAll()
                    .Include(p => p.CreatorUser)
                    .Include(p => p.ProjectTask)
                    .Include("ProjectTask.CreatorUser")
                    .Include("ProjectTask.AgentUser")
                    .Include(p => p.TaskLogAttachments)
                    .Include("TaskLogAttachments.Attachment")
                    .WhereIf(input.CreatorUserId != null, t => t.CreatorUserId == input.CreatorUserId)
                    .WhereIf(input.TaskId != null, t => t.TaskId == input.TaskId)
                    .WhereIf(!string.IsNullOrEmpty(input.Filter), t => t.Notes.Contains(input.Filter));

            var totalCount = await query.CountAsync();
            var taskLogs = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var toReturn = taskLogs.MapTo<List<TaskLogWithTaskAndAttachmentsListDto>>();
            return new PagedResultOutput<TaskLogWithTaskAndAttachmentsListDto>(totalCount, toReturn);
        }
        public ListResultOutput<TaskLogAttachmentListDto> GetTaskLogAttachments(IdInput input)
        {
            var toReturn = _taskLogAttachmentRepository.GetAll().Include(x => x.Attachment).Where(x => x.TaskLogId == input.Id).ToList();
            return new ListResultOutput<TaskLogAttachmentListDto>(toReturn.MapTo<List<TaskLogAttachmentListDto>>());
        }

        [AbpAuthorize(AppPermissions.Pages_TaskTracker_Agent)]
        public async Task DeleteTaskLogAttachment(IdInput input)
        {
            await _taskLogAttachmentRepository.DeleteAsync(input.Id);
        }

        #endregion

        #region ToDos

        public ToDoListDto GetToDo(IdInput input)
        {
            var toReturn = _todoRepository.Get(input.Id);
            return toReturn.MapTo<ToDoListDto>();
        }

        public ListResultOutput<ToDoListDto> GetToDos(GetToDosInput input)
        {
            var toReturn =
                _todoRepository.GetAll()
                    .Where(x => x.CreatorUserId == input.CreatorUserId)
                    .Where(x => x.IsComplete == input.IsComplete)
                    .WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Title.Contains(input.Filter))
                    .ToList();
            return new ListResultOutput<ToDoListDto>(toReturn.MapTo<List<ToDoListDto>>());
        }

        public async Task CreapdateToDo(CreapdateToDoInput input)
        {
            if (input.Id == null)
            {
                // create
                var toCreate = input.MapTo<ToDo>();
                await _todoRepository.InsertAsync(toCreate);
            }
            else
            {
                // update
                var toUpdate = await _todoRepository.GetAsync((int)input.Id);
                input.MapTo(toUpdate);
            }
        }

        public async Task MarkToDoComplete(IdInput input)
        {
            var toDo = await _todoRepository.GetAsync(input.Id);
            toDo.IsComplete = true;
        }

        public async Task DeleteToDo(IdInput input)
        {
            await _todoRepository.DeleteAsync(input.Id);
        }


        #endregion

        #region Private methods

        private async Task NotifyAgent(TaskListDto task, string subject)
        {
            var user = await _userManager.GetUserByIdAsync((int)task.AgentId);
            var recipients = user.EmailAddress;
            mailer.TaskAgentNotified(task, recipients, subject).Send();
        }

        private void NotifyManagers(ProjectTask task, string subject)
        {
            var managers = GetProjectWithTasksAndManagers(new IdInput { Id = task.ProjectId }).Managers;
            var recipients = managers.Aggregate(string.Empty, (current, manager) => current + (manager.User.EmailAddress + ","));
            mailer.TaskManagerNotified(task.MapTo<TaskListDto>(), recipients.Remove(recipients.Length - 1), subject).Send();
        }

        private void NotifyManagersForProject(ProjectWithManagersListDto input, string subject)
        {
            var managers = GetProjectWithTasksAndManagers(new IdInput { Id = input.Id }).Managers;
            var recipients = managers.Aggregate(string.Empty, (current, manager) => current + (manager.User.EmailAddress + ","));
            mailer.TaskManagerNotifiedForProject(input, recipients.Remove(recipients.Length - 1), subject).Send();
        }

        #endregion

        #region deleted

        //[AbpAuthorize(AppPermissions.Pages_TaskTracker_Manager)]
        //public async Task UpdateProjectsStatus(IdInput input)
        //{
        //    // update project
        //    var projectWithOpenedTasks = await _projectRepository.GetAll()
        //            .Include(p => p.Tasks)
        //            .Where(x => x.Tasks.Any(y => y.Status == TaskTracker.Enum.Status.Open))
        //            .Where(x => x.Id == input.Id).FirstOrDefaultAsync();

        //    if (projectWithOpenedTasks != null)
        //    {
        //        projectWithOpenedTasks.Status = TaskTracker.Enum.Status.Open;
        //        _projectRepository.Update(projectWithOpenedTasks);
        //    }
        //    else
        //    {
        //        var toUpdate = await _projectRepository.GetAsync(input.Id);
        //        toUpdate.Status = TaskTracker.Enum.Status.Closed;
        //        _projectRepository.Update(toUpdate);
        //    }
        //}

        #endregion
    }
}