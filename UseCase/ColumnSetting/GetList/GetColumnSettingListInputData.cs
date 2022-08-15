using UseCase.Core.Sync.Core;

namespace UseCase.ColumnSetting.GetList;

public class GetColumnSettingListInputData : IInputData<GetColumnSettingListOutputData>
{
    public GetColumnSettingListInputData(int userId, string tableName)
    {
        UserId = userId;
        TableName = tableName;
    }

    public int UserId { get; private set; }
    public string TableName { get; private set; }
}
