using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table(name: "sinreki_filter_mst_koui")]
public class SinrekiFilterMstKoui : EmrCloneable<SinrekiFilterMstKoui>
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(name: "hp_id", Order = 1)]
    public int HpId { get; set; }

    [Column(name: "grp_cd", Order = 2)]
    public int GrpCd { get; set; }

    [Column(name: "seq_no", Order = 3)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SeqNo { get; set; }

    [Column(name: "koui_kbn_id", Order = 4)]
    public int KouiKbnId { get; set; }

    [Column(name: "is_deleted", Order = 5)]
    [CustomAttribute.DefaultValue(0)]
    public int IsDeleted { get; set; }

    [Column("create_date", Order = 6)]
    public DateTime CreateDate { get; set; }

    [Column("create_id", Order = 7)]
    [CustomAttribute.DefaultValue(0)]
    public int CreateId { get; set; }

    [Column("create_machine", Order = 8)]
    [MaxLength(60)]
    public string? CreateMachine { get; set; } = string.Empty;

    [Column("update_date", Order = 9)]
    public DateTime UpdateDate { get; set; }

    [Column("update_id", Order = 10)]
    [CustomAttribute.DefaultValue(0)]
    public int UpdateId { get; set; }

    [Column("update_machine", Order = 11)]
    [MaxLength(60)]
    public string? UpdateMachine { get; set; } = string.Empty;
}
