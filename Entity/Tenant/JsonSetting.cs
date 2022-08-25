using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table("JSON_SETTING")]
public class JsonSetting
{
    [Column("USER_ID")]
    public int UserId { get; set; }

    [Column("KEY")]
    public string Key { get; set; } = null!;

    [Column("VALUE")]
    public string Value { get; set; } = null!;
}
