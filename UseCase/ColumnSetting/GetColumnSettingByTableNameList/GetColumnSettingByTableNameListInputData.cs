using UseCase.Core.Sync.Core;

namespace UseCase.ColumnSetting.GetColumnSettingByTableNameList;

public class GetColumnSettingByTableNameListInputData : IInputData<GetColumnSettingByTableNameListOutputData>
{
    public GetColumnSettingByTableNameListInputData(int userId, List<string> tableNameList)
    {
        UserId = userId;
        TableNameList = tableNameList;
    }

    public int UserId { get; private set; }

    public List<string> TableNameList { get; private set; }
}
