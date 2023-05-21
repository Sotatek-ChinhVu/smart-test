using UseCase.Core.Sync.Core;

namespace UseCase.KarteInf.GetList;

public class GetListKarteInfInputData : IInputData<GetListKarteInfOutputData>
{
    public GetListKarteInfInputData(int hpId, long ptId, long raiinNo, int sinDate, bool isDeleted, int userId)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
        IsDeleted = isDeleted;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }

    public bool IsDeleted { get; private set; }

    public int UserId { get; private set; }
}
