namespace Domain.SuperAdminModels.SystemChangeLog;

public interface ISystemChangeLogRepository
{
    void AddSystemChangeLog(SystemChangeLogModel model);

    void ReleaseResource();
}
