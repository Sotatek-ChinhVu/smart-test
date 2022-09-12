using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetList;

public class GetReceptionListInputData : IInputData<GetReceptionListOutputData>
{
    public GetReceptionListInputData(int hpId, int sinDate, long raiinNo, long ptId)
    {
        HpId = hpId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        PtId = ptId;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public long RaiinNo { get; private set; }
    public long PtId { get; private set; }
}
