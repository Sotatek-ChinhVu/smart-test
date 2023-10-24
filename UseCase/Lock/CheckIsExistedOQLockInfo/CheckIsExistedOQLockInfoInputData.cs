using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckIsExistedOQLockInfo;

public class CheckIsExistedOQLockInfoInputData : IInputData<CheckIsExistedOQLockInfoOutputData>
{
    public CheckIsExistedOQLockInfoInputData(int hpId, int userId, long ptId, string functionCd, long raiinNo, int sinDate)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        FunctionCd = functionCd;
        RaiinNo = raiinNo;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public string FunctionCd { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }
}
