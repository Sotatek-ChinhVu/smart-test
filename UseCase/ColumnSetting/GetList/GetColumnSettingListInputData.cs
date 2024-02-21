using UseCase.Core.Sync.Core;

namespace UseCase.ColumnSetting.GetList;

public class GetColumnSettingListInputData : IInputData<GetColumnSettingListOutputData>
{
    public GetColumnSettingListInputData(int hpId, int userId, string tableName)
    {
        HpId = hpId;
        UserId = userId;
        TableName = tableName;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public string TableName { get; private set; }
}
