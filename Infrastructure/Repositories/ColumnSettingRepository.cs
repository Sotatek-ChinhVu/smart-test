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

    private ColumnSettingModel ToModel(ColumnSetting c)
    {
        return new ColumnSettingModel(c.UserId, c.TableName,
            c.ColumnName, c.DisplayOrder, c.IsPinned, c.IsHidden, c.Width);
    }
}
