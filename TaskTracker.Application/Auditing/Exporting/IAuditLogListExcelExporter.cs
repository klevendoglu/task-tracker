using System.Collections.Generic;
using TaskTracker.Auditing.Dto;
using TaskTracker.Dto;

namespace TaskTracker.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
