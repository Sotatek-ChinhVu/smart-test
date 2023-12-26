using Domain.SuperAdminModels.SystemChangeLog;
using Entity.SuperAdmin;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Helper.Common;
using Infrastructure.Services;
using Domain.Constant;
using static StackExchange.Redis.Role;
using System.ComponentModel;

namespace Infrastructure.SuperAdminRepositories;

public class SystemChangeLogRepository : SuperAdminRepositoryBase, ISystemChangeLogRepository
{
    public SystemChangeLogRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void SaveSystemChangeLog(SystemChangeLogModel model)
    {
        var logEntity = TrackingDataContext.SystemChangeLogs.FirstOrDefault(item => item.Id == model.Id);
        if (logEntity == null)
        {
            logEntity = new SystemChangeLog()
            {
                Id = 0,

                CreateDate = CIUtil.GetJapanDateTimeNow(),
            };
        }
        logEntity.FileName = model.FileName;
        logEntity.Version = model.Version;
        logEntity.IsPg = model.IsPg;
        logEntity.IsDb = model.IsDb;
        logEntity.IsMaster = model.IsMaster;
        logEntity.IsRun = model.IsRun;
        logEntity.IsNote = model.IsNote;
        logEntity.IsDrugPhoto = model.IsDrugPhoto;
        logEntity.Status = model.Status;
        logEntity.ErrMessage = model.ErrMessage;
        if (logEntity.Id == 0)
        {
            TrackingDataContext.SystemChangeLogs.Add(logEntity);
        }
        TrackingDataContext.SaveChanges();
    }

    public SystemChangeLogModel GetSystemChangeLog(string fileName, string ver)
    {
        var entity = NoTrackingDataContext.SystemChangeLogs.FirstOrDefault(item => item.Version == ver && item.FileName == fileName);
        if (entity != null)
        {
            return new SystemChangeLogModel(
                       entity.Id,
                       entity.FileName ?? string.Empty,
                       entity.Version ?? string.Empty,
                       entity.IsPg,
                       entity.IsDb,
                       entity.IsMaster,
                       entity.IsRun,
                       entity.IsNote,
                       entity.IsDrugPhoto,
                       entity.Status,
                       entity.ErrMessage ?? string.Empty);
        }
        return new SystemChangeLogModel(fileName, ver);
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
