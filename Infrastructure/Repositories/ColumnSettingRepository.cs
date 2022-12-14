using Domain.Models.ColumnSetting;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class ColumnSettingRepository : IColumnSettingRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public ColumnSettingRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
    }

    public List<ColumnSettingModel> GetList(int userId, string tableName)
    {
        return _tenantDataContext.ColumnSettings
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

        var existingSettings = _tenantDataContext.ColumnSettings
            .Where(c => c.UserId == userId && c.TableName == tableName).ToList();
        _tenantDataContext.ColumnSettings.RemoveRange(existingSettings);

        var newSettings = settingModels.Select(m => ToEntity(m));
        _tenantDataContext.ColumnSettings.AddRange(newSettings);

        _tenantDataContext.SaveChanges();
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
}
