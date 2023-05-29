using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetList;

public class GetReceptionListInputData : IInputData<GetReceptionListOutputData>
{
    public GetReceptionListInputData(int hpId, int sinDate, long raiinNo, long ptId, bool isGetFamily, int isDeleted, bool searchSameVisit)
    {
        HpId = hpId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        PtId = ptId;
        IsGetFamily = isGetFamily;
        IsDeleted = isDeleted;
        SearchSameVisit = searchSameVisit;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public long RaiinNo { get; private set; }
    public long PtId { get; private set; }
    public bool IsGetFamily { get; private set; }
    public int IsDeleted { get; private set; }
    public bool SearchSameVisit { get; private set; }
}
