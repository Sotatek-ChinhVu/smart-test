using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.GetAccountDueList;

public class GetAccountDueListInputData : IInputData<GetAccountDueListOutputData>
{
    public GetAccountDueListInputData(int hpId, long ptId, int sinDate, bool isUnpaidChecked)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        IsUnpaidChecked = isUnpaidChecked;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public bool IsUnpaidChecked { get; private set; }
}
