using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.GetAccountDueList;

public class GetAccountDueListInputData : IInputData<GetAccountDueListOutputData>
{
    public GetAccountDueListInputData(int hpId, long ptId, int sinDate, bool isUnpaidChecked, int pageIndex, int pageSize)
    {
        this.hpId = hpId;
        this.ptId = ptId;
        this.sinDate = sinDate;
        this.isUnpaidChecked = isUnpaidChecked;
        this.pageIndex = pageIndex;
        this.pageSize = pageSize;
    }

    public int hpId { get; private set; }

    public long ptId { get; private set; }

    public int sinDate { get; private set; }

    public bool isUnpaidChecked { get; private set; }

    public int pageIndex { get; private set; }

    public int pageSize { get; private set; }
}
