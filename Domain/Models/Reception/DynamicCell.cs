using Helper.Constants;

namespace Domain.Models.Reception;

public class DynamicCell
{
    public DynamicCell(int grpId)
    {
        GrpId = grpId;
        KbnCd = CommonConstants.KbnCdDeleteFlag;
    }

    public DynamicCell(int grpId, int kbnCd, string kbnName, string colorCd)
    {
        GrpId = grpId;
        KbnCd = kbnCd;
        KbnName = kbnName;
        ColorCd = colorCd;
    }

    public int GrpId { get; set; }
    public int KbnCd { get; set; }
    public string KbnName { get; set; } = string.Empty;
    public string ColorCd { get; set; } = string.Empty;
}
