using Domain.SuperAdminModels.SystemChangeLog;
using Entity.SuperAdmin;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Helper.Common;

namespace Infrastructure.SuperAdminRepositories;

public class SystemChangeLogRepository : SuperAdminRepositoryBase, ISystemChangeLogRepository
{
    public SystemChangeLogRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void AddSystemChangeLog(SystemChangeLogModel model)
    {
        var logEntity = new SystemChangeLog()
        {
            FileName = model.FileName,
            Version = model.Version,
            IsPg = model.IsPg,
            IsDb = model.IsDb,
            IsMaster = model.IsMaster,
            IsRun = model.IsRun,
            IsNote = model.IsNote,
            IsDrugPhoto = model.IsDrugPhoto,
            Status = model.Status,
            ErrMessage = model.ErrMessage,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
        };
        TrackingDataContext.SystemChangeLogs.Add(logEntity);
        TrackingDataContext.SaveChanges();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
