using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.SaveAccountDueList;

public class SaveAccountDueListInputData : IInputData<SaveAccountDueListOutputData>
{
    public SaveAccountDueListInputData(int hpId, int userId, long ptId, int sinDate, string kaikeiTime, List<SyunoNyukinInputItem> syunoNyukinInputItems)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinDate = sinDate;
        KaikeiTime = kaikeiTime;
        SyunoNyukinInputItems = syunoNyukinInputItems;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public string KaikeiTime { get; private set; }

    public List<SyunoNyukinInputItem> SyunoNyukinInputItems { get; private set; }
}
