using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateDynamicCell;

public class UpdateReceptionDynamicCellInputData : IInputData<UpdateReceptionDynamicCellOutputData>
{
    public UpdateReceptionDynamicCellInputData(int hpId, int sinDate, long raiinNo, long ptId, int grpId, int kbnCd, int userId)
    {
        HpId = hpId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        PtId = ptId;
        GrpId = grpId;
        KbnCd = kbnCd;
        UserId = userId;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public long RaiinNo { get; private set; }
    public long PtId { get; private set; }
    public int GrpId { get; private set; }
    public int KbnCd { get; private set; }
    public int UserId { get; private set; }
}
