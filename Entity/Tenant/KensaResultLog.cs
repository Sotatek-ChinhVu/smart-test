using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Tenant;

[Table(name: "KENSA_RESULT_LOG")]
[Serializable]
[Index(nameof(HpId), nameof(ImpDate), Name = "KENSA_RESULT_LOG_IDX01")]
public class KensaResultLog
{
    [Column("OP_ID", Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OpId { get; set; }

    [Column("HP_ID", Order = 2)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int HpId { get; set; }

    [Column("IMP_DATE", Order = 3)]
    public int ImpDate { get; set; }

    [Column("KEKA_FILE", Order = 4)]
    public string? KekaFile { get; set; } = string.Empty;

    [Column("CREATE_DATE")]
    public DateTime CreateDate { get; set; }

    [Column("CREATE_ID")]
    public int CreateId { get; set; }

    [Column("CREATE_MACHINE")]
    [MaxLength(60)]
    public string? CreateMachine { get; set; } = string.Empty;
}