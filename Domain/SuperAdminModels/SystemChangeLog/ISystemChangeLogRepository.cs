namespace Domain.SuperAdminModels.SystemChangeLog;

public interface ISystemChangeLogRepository
{
    void SaveSystemChangeLog(SystemChangeLogModel model);

    SystemChangeLogModel GetSystemChangeLog(string fileName, string ver);

    void ReleaseResource();
}
