using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table(name: "SINREKI_FILTER_MST_KOUI")]
public class SinrekiFilterMstKoui : EmrCloneable<SinrekiFilterMstKoui>
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(name: "HP_ID", Order = 1)]
    public long HpId { get; set; }

    [Column(name: "GRP_CD", Order = 2)]
    public int GrpCd { get; set; }

    [Column(name: "ID", Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
}
