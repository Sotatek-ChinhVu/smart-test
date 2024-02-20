using UseCase.Core.Sync.Core;

namespace UseCase.ColumnSetting.GetColumnSettingByTableNameList;

public class GetColumnSettingByTableNameListInputData : IInputData<GetColumnSettingByTableNameListOutputData>
{
    public GetColumnSettingByTableNameListInputData(int hpId, int userId, List<string> tableNameList)
    {
        HpId = hpId;
        UserId = userId;
        TableNameList = tableNameList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<string> TableNameList { get; private set; }
}
