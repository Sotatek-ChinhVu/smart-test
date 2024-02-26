namespace Domain.Models.ColumnSetting;

public class ColumnSettingModel
{
    public ColumnSettingModel(int hpId, int userId, string tableName, string columnName,
        int displayOrder, bool isPinned, bool isHidden, int width, string orderBy)
    {
        HpId = hpId;
        UserId = userId;
        TableName = tableName;
        ColumnName = columnName;
        DisplayOrder = displayOrder;
        IsPinned = isPinned;
        IsHidden = isHidden;
        Width = width;
        OrderBy = orderBy;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public string TableName { get; private set; }

    public string ColumnName { get; private set; }

    public int DisplayOrder { get; private set; }

    public bool IsPinned { get; private set; }

    public bool IsHidden { get; private set; }

    public int Width { get; private set; }

    public string OrderBy { get; private set; }
}
