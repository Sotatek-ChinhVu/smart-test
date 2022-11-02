using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.GetAccountDueList;

public class GetAccountDueListInputData : IInputData<GetAccountDueListOutputData>
{
    public GetAccountDueListInputData(int hpId, long ptId, int sinDate, bool isUnpaidChecked, int pageIndex, int pageSize)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        IsUnpaidChecked = isUnpaidChecked;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public bool IsUnpaidChecked { get; private set; }

    public int PageIndex { get; private set; }

    public int PageSize { get; private set; }
}
