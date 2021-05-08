using System.Collections.Generic;
using TaskTracker.Authorization.Users.Dto;
using TaskTracker.Dto;

namespace TaskTracker.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}