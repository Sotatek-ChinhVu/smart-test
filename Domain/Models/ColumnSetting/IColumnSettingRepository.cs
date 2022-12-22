using Domain.Common;

namespace Domain.Models.ColumnSetting;

public interface IColumnSettingRepository : IRepositoryBase
{
    List<ColumnSettingModel> GetList(int userId, string tableName);
    bool SaveList(List<ColumnSettingModel> settingModels);
}
