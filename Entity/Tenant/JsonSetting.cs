using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table("json_setting")]
public class JsonSetting
{
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("key")]
    public string Key { get; set; } = null!;

    [Column("value")]
    public string Value { get; set; } = null!;
}
