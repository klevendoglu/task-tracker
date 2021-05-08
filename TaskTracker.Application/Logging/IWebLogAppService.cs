using Abp.Application.Services;
using TaskTracker.Dto;
using TaskTracker.Logging.Dto;

namespace TaskTracker.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
