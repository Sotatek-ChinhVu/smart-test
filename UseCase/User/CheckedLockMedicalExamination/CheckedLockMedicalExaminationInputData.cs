using UseCase.Core.Sync.Core;

namespace UseCase.User.CheckedLockMedicalExamination;

public class CheckedLockMedicalExaminationInputData : IInputData<CheckedLockMedicalExaminationOutputData>
{
    public CheckedLockMedicalExaminationInputData(int hpId, long ptId, long raiinNo, int sinDate, string token, int userId)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
        Token = token;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }

    public string Token { get; private set; }

    public int UserId { get; private set; }
}
