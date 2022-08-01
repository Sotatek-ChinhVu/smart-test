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

    public int GrpId { get; private set; }
    public int KbnCd { get; private set; }
    public string KbnName { get; private set; } = string.Empty;
    public string ColorCd { get; private set; } = string.Empty;
}
