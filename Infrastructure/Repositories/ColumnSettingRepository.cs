using Domain.Models.ColumnSetting;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class ColumnSettingRepository : RepositoryBase, IColumnSettingRepository
{
    public ColumnSettingRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<ColumnSettingModel> GetList(int userId, string tableName)
    {
        return NoTrackingDataContext.ColumnSettings
            .Where(c => c.UserId == userId && c.TableName == tableName)
            .AsEnumerable().Select(c => ToModel(c)).ToList();
    }

    public bool SaveList(List<ColumnSettingModel> settingModels)
    {
        if (settingModels.Count == 0)
        {
            return true;
        }

        var userId = settingModels.First().UserId;
        var tableName = settingModels.First().TableName;

        var unrelatedSetting = settingModels.FirstOrDefault(m => m.UserId != userId || m.TableName != tableName);
        if (unrelatedSetting is not null)
        {
            return false;
        }

        var existingSettings = TrackingDataContext.ColumnSettings
            .Where(c => c.UserId == userId && c.TableName == tableName).ToList();
        TrackingDataContext.ColumnSettings.RemoveRange(existingSettings);

        var newSettings = settingModels.Select(m => ToEntity(m));
        TrackingDataContext.ColumnSettings.AddRange(newSettings);

        TrackingDataContext.SaveChanges();
        return true;
    }

    private ColumnSettingModel ToModel(ColumnSetting c)
    {
        return new ColumnSettingModel(c.UserId, c.TableName,
            c.ColumnName, c.DisplayOrder, c.IsPinned, c.IsHidden, c.Width);
    }

    private ColumnSetting ToEntity(ColumnSettingModel model)
    {
        return new ColumnSetting
        {
            UserId = model.UserId,
            TableName = model.TableName,
            ColumnName = model.ColumnName,
            DisplayOrder = model.DisplayOrder,
            IsPinned = model.IsPinned,
            IsHidden = model.IsHidden,
            Width = model.Width
        };
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
