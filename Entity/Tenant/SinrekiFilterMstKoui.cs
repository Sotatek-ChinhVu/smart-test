using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table(name: "SINREKI_FILTER_MST_KOUI")]
public class SinrekiFilterMstKoui : EmrCloneable<SinrekiFilterMstKoui>
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(name: "HP_ID", Order = 1)]
    public int HpId { get; set; }

    [Column(name: "GRP_CD", Order = 2)]
    public int GrpCd { get; set; }

    [Column(name: "SEQ_NO", Order = 3)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SeqNo { get; set; }

    [Column(name: "KOUI_KBN_ID", Order = 4)]
    public int KouiKbnId { get; set; }

    [Column(name: "IS_DELETED", Order = 5)]
    [CustomAttribute.DefaultValue(0)]
    public int IsDeleted { get; set; }

    [Column("CREATE_DATE", Order = 6)]
    public DateTime CreateDate { get; set; }

    [Column("CREATE_ID", Order = 7)]
    [CustomAttribute.DefaultValue(0)]
    public int CreateId { get; set; }

    [Column("CREATE_MACHINE", Order = 8)]
    [MaxLength(60)]
    public string? CreateMachine { get; set; } = string.Empty;

    [Column("UPDATE_DATE", Order = 9)]
    public DateTime UpdateDate { get; set; }

    [Column("UPDATE_ID", Order = 10)]
    [CustomAttribute.DefaultValue(0)]
    public int UpdateId { get; set; }

    [Column("UPDATE_MACHINE", Order = 11)]
    [MaxLength(60)]
    public string? UpdateMachine { get; set; } = string.Empty;
}
