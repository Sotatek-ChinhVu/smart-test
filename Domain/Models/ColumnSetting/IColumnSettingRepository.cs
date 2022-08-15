namespace Domain.Models.ColumnSetting;

public interface IColumnSettingRepository
{
    List<ColumnSettingModel> GetList(int userId, string tableName);
    bool SaveList(List<ColumnSettingModel> settingModels);
}
