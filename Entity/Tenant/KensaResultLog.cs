using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Tenant;

[Table(name: "kensa_result_log")]
[Serializable]
[Index(nameof(HpId), nameof(ImpDate), Name = "kensa_result_log_idx01")]
public class KensaResultLog
{
    [Column("op_id", Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OpId { get; set; }

    [Column("hp_id", Order = 2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int HpId { get; set; }

    [Column("imp_date", Order = 3)]
    public int ImpDate { get; set; }

    [Column("keka_file", Order = 4)]
    public string? KekaFile { get; set; } = string.Empty;

    [Column("create_date")]
    public DateTime CreateDate { get; set; }

    [Column("create_id")]
    public int CreateId { get; set; }

    [Column("create_machine")]
    [MaxLength(60)]
    public string? CreateMachine { get; set; } = string.Empty;
}