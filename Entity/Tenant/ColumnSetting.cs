using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table("COLUMN_SETTING")]
public class ColumnSetting
{
    [Column("USER_ID")]
    public int UserId { get; set; }

    [Column("TABLE_NAME")]
    public string TableName { get; set; } = null!;

    [Column("COLUMN_NAME")]
    public string ColumnName { get; set; } = null!;

    [Column("DISPLAY_ORDER")]
    public int DisplayOrder { get; set; }

    [Column("IS_PINNED")]
    public bool IsPinned { get; set; }

    [Column("IS_HIDDEN")]
    public bool IsHidden { get; set; }

    [Column("WIDTH")]
    public int Width { get; set; }
}
