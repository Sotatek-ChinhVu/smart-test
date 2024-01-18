using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table("column_setting")]
public class ColumnSetting
{
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("table_name")]
    public string TableName { get; set; } = null!;

    [Column("column_name")]
    public string ColumnName { get; set; } = null!;

    [Column("display_order")]
    public int DisplayOrder { get; set; }

    [Column("is_pinned")]
    public bool IsPinned { get; set; }

    [Column("is_hidden")]
    public bool IsHidden { get; set; }

    [Column("width")]
    public int Width { get; set; }

    [Column("order_by")]
    public string OrderBy { get; set; } = null!;
}
