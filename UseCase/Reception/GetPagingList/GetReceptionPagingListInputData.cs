using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetPagingList;

public class GetReceptionPagingListInputData : IInputData<GetReceptionPagingListOutputData>
{
    public GetReceptionPagingListInputData(int hpId, int sinDate, long raiinNo, long ptId, bool isGetFamily, int isDeleted, bool searchSameVisit, int limit, int offset)
    {
        HpId = hpId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        PtId = ptId;
        IsGetFamily = isGetFamily;
        IsDeleted = isDeleted;
        SearchSameVisit = searchSameVisit;
        Limit = limit;
        Offset = offset;
    }

    public int HpId { get; private set; }
    public int SinDate { get; private set; }
    public long RaiinNo { get; private set; }
    public long PtId { get; private set; }
    public bool IsGetFamily { get; private set; }
    public int IsDeleted { get; private set; }
    public bool SearchSameVisit { get; private set; }
    public int Limit { get; private set; }
    public int Offset { get; private set; }
}
